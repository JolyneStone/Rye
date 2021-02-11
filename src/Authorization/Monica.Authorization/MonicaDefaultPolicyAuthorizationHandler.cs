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

using Monica.Authorization.Abstraction.Attributes;
using Monica.Authorization.Entities;
using Monica.Cache;
using Monica.Cache.Internal;
using Monica.DataAccess;
using Monica.Jwt;
using Monica.Jwt.Options;
using Monica.Web.Options;
using Monica.Web.ResponseProvider.Authorization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Monica.Authorization.Abstraction
{
    public class MonicaDefaultPolicyAuthorizationHandler : MonicaPolicyAuthorizationHandler
    {
        private IAuthorizationResponseProvider _provider;
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
            if (_provider == null)
            {
                _provider = services.GetRequiredService<IOptions<MonicaWebOptions>>().Value.Authorization.Provider;
            }

            var (validResult, principal) = await ValidTokenAsync(context, httpContext);
            if (!validResult)
                return false;

            httpContext.User = principal;
            var userId = httpContext.User?.Claims.FirstOrDefault(c => c.Type == nameof(PermissionTokenEntity.UserId))?.Value;
            if (userId.IsNullOrEmpty())
            {
                await WriteResponseAsync(httpContext, _provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return false;
            }
            var loginAttribute = endpointMetadata.GetMetadata<LoginAttribute>(); // 登录用户
            if (loginAttribute != null)
            {
                return true;
            }

            var url = httpContext.Request.Path.Value.ToLower();
            var roleIds = httpContext.User.Claims.FirstOrDefault(c => c.Type == nameof(PermissionTokenEntity.RoleIds))?.Value;
            var secutiryPermissionService = services.GetRequiredService<ISecutiryPermissionService>();
            if (roleIds.IsNullOrEmpty())
            {
                await WriteResponseAsync(httpContext, _provider.CreatePermissionNotAllowResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return false;
            }

            var cacheService = services.GetRequiredService<ICacheService>();
            var entry = MonicaCacheEntryCollection.GetPermissionEntry(userId);
            IEnumerable<string> permissionList = await cacheService.GetAsync<IEnumerable<string>>(entry.CacheKey);
            if (permissionList == null || !permissionList.Any())
            {
                permissionList = await secutiryPermissionService.GetPermissionCodeAsync(roleIds);
                if (permissionList != null && permissionList.Any())
                {
                    await cacheService.SetAsync(entry.CacheKey, permissionList, entry.Options);
                }
            }

            var authCode = httpContext.GetRouteValue("action").ToString() ?? string.Empty;
            var authCodeAttribute = endpointMetadata.GetMetadata<AuthCodeAttribute>();
            if (authCodeAttribute != null && !string.IsNullOrEmpty(authCodeAttribute.AuthCode))
            {
                authCode = authCodeAttribute.AuthCode;
            }

            if (!permissionList.Any(d => string.Equals(d, authCode, System.StringComparison.OrdinalIgnoreCase)))
            {
                await WriteResponseAsync(httpContext, _provider.CreatePermissionNotAllowResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return false;
            }

            return true;
        }

        private async Task<(bool, ClaimsPrincipal)> ValidTokenAsync(AuthorizationHandlerContext context, HttpContext httpContext)
        {
            ClaimsPrincipal principal = null;
            var services = httpContext.RequestServices;
            var tokenService = services.GetRequiredService<IJwtTokenService>();
            var jwtOptions = services.GetRequiredService<IOptions<JwtOptions>>().Value;
            var logger = services.GetRequiredService<ILogger<MonicaDefaultPolicyAuthorizationHandler>>();
            if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationToken))
            {
                await WriteResponseAsync(httpContext, _provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return (false, principal);
            }

            try
            {
                principal = tokenService.ValidateToken(authorizationToken.ToString().Substring(jwtOptions.Scheme.Length + 1));
            }
            catch (SecurityTokenInvalidLifetimeException lifetimeEx)
            {
                logger.LogError(lifetimeEx.ToString());
                await WriteResponseAsync(httpContext, _provider.CreateTokenExpireResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return (false, principal);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                await WriteResponseAsync(httpContext, _provider.CreateTokenErrorResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return (false, principal);
            }
            var tokenTypeStr = httpContext.User?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;
            if (!tokenTypeStr.IsNullOrEmpty() && tokenTypeStr.ParseByInt() != (int)JwtTokenType.AccessToken)
            {
                logger.LogError("JwtTokenType error");
                await WriteResponseAsync(httpContext, _provider.CreateTokenErrorResponse(new AuthorizationResponseContext(context)).Value);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
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
