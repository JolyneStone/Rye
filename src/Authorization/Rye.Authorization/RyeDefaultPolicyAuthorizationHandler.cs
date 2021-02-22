using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Rye.Authorization.Abstraction.Attributes;
using Rye.Authorization.Entities;
using Rye.Cache;
using Rye.Cache.Internal;
using Rye.DataAccess;
using Rye.Entities.Abstractions;
using Rye.Jwt;
using Rye.Jwt.Options;
using Rye.Web.Options;
using Rye.Web.ResponseProvider.Authorization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rye.Authorization.Abstraction
{
    public class RyeDefaultPolicyAuthorizationHandler : RyePolicyAuthorizationHandler
    {
        /// <summary>
        /// 根据用户角色进行权限认证
        /// </summary>
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
        protected override async Task<bool> AuthorizeCoreAsync(AuthorizationHandlerContext context)
        {
            var httpContext = context.Resource as HttpContext;
            var endpointMetadata = httpContext.Features.Get<IEndpointFeature>()?.Endpoint.Metadata;
            var allowAnonymousAttribute = endpointMetadata.GetMetadata<AllowAnonymousAttribute>();
            if (allowAnonymousAttribute != null)
            {
                return true;
            }

            var services = httpContext.RequestServices;
            var provider = services.GetRequiredService<IOptions<RyeWebOptions>>().Value.Authorization.Provider;

            AuthCodeAttribute authCodeAttribute = endpointMetadata.GetMetadata<AuthCodeAttribute>();
            LoginAttribute loginAttribute = authCodeAttribute != null ? null : endpointMetadata.GetMetadata<LoginAttribute>();
            TokenValidAttribute tokenValidAttribute = authCodeAttribute != null || loginAttribute != null ?
                null :
                endpointMetadata.GetMetadata<TokenValidAttribute>();

            if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationToken))
            {
                if (tokenValidAttribute != null)
                {
                    return true;
                }
                await WriteResponseAsync(httpContext, provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return false;
            }
            var jwtOptions = services.GetRequiredService<IOptions<JwtOptions>>().Value;
            var token = authorizationToken.ToString().Substring(jwtOptions.Scheme.Length + 1);
            var (validResult, principal) = await ValidTokenAsync(context, httpContext, provider, token);
            if (!validResult)
                return false;

            if (tokenValidAttribute != null)
            {
                return true;
            }

            httpContext.User = principal;
            var userId = httpContext.User?.Claims.FirstOrDefault(c => c.Type == nameof(PermissionTokenEntity.UserId))?.Value;
            if (userId.IsNullOrEmpty())
            {
                await WriteResponseAsync(httpContext, provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return false;
            }

            if (loginAttribute != null)
            {
                return true;
            }

            if (authCodeAttribute == null)
            {
                return true;
            }
            var url = httpContext.Request.Path.Value.ToLower();
            var roleIds = httpContext.User.Claims.FirstOrDefault(c => c.Type == nameof(PermissionTokenEntity.RoleIds))?.Value;
            var secutiryPermissionService = services.GetRequiredService<IPermissionService>();
            if (roleIds.IsNullOrEmpty())
            {
                await WriteResponseAsync(httpContext, provider.CreatePermissionNotAllowResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return false;
            }

            var cacheService = services.GetRequiredService<ICacheService>();
            var entry = CacheEntryCollection.GetPermissionEntry(userId);
            IEnumerable<string> permissionList = await cacheService.GetAsync<IEnumerable<string>>(entry.CacheKey);
            if (permissionList == null || !permissionList.Any())
            {
                permissionList = await secutiryPermissionService.GetPermissionCodeAsync(roleIds);
                if (permissionList != null && permissionList.Any())
                {
                    await cacheService.SetAsync(entry.CacheKey, permissionList, entry.Options);
                }
            }

            var authCode = authCodeAttribute.AuthCode ?? httpContext.GetRouteValue("action").ToString() ?? string.Empty;
            if (!permissionList.Any(d => string.Equals(d, authCode, System.StringComparison.OrdinalIgnoreCase)))
            {
                await WriteResponseAsync(httpContext, provider.CreatePermissionNotAllowResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return false;
            }

            return true;
        }

        private static async Task<(bool, ClaimsPrincipal)> ValidTokenAsync(
            AuthorizationHandlerContext context,
            HttpContext httpContext,
            IAuthorizationResponseProvider provider,
            string token)
        {
            ClaimsPrincipal principal = null;
            var services = httpContext.RequestServices;
            var tokenService = services.GetRequiredService<IJwtTokenService>();
            var logger = services.GetRequiredService<ILogger<RyeDefaultPolicyAuthorizationHandler>>();

            try
            {
                principal = await tokenService.ValidateTokenAsync(JwtTokenType.AccessToken, token);
            }
            catch (SecurityTokenInvalidLifetimeException lifetimeEx)
            {
                logger.LogError(lifetimeEx.ToString());
                await WriteResponseAsync(httpContext, provider.CreateTokenExpireResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return (false, principal);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                await WriteResponseAsync(httpContext, provider.CreateTokenErrorResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return (false, principal);
            }
            var tokenTypeStr = httpContext.User?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;
            if (tokenTypeStr.IsNullOrEmpty() || tokenTypeStr.ParseByInt() != (int)JwtTokenType.AccessToken)
            {
                logger.LogError("JwtTokenType error");
                await WriteResponseAsync(httpContext, provider.CreateTokenErrorResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return (false, principal);
            }

            return (true, principal);
        }

        private static async Task WriteResponseAsync(HttpContext context, object value)
        {
            // 保持原来的流
            var originalBody = context.Response.Body;
            await originalBody.FlushAsync();
            var ms = new MemoryStream();
            ms.Seek(0, SeekOrigin.Begin);
            var buffer = Encoding.UTF8.GetBytes(value.ToJsonString());
            // 写入到原有的流中
            await ms.WriteAsync(buffer);
            ms.Seek(0, SeekOrigin.Begin);
            await ms.CopyToAsync(originalBody);
        }
    }
}
