using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Rye.Authorization.Entities;
using Rye.Cache;
using Rye.Cache.Store;
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
using System.Threading.Tasks;

namespace Rye.Authorization.Abstraction
{
    public class RyeDefaultPolicyAuthorizationHandler<TPermissionKey> : RyePolicyAuthorizationHandler
        where TPermissionKey : IEquatable<TPermissionKey>
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

            if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationTokenVal))
            {
                if (tokenValidAttribute != null)
                {
                    return true;
                }
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
                return false;
            }
            var jwtOptions = services.GetRequiredService<IOptions<JwtOptions>>().Value;
            var authorizationToken = authorizationTokenVal.ToString();
            if (authorizationToken.Length < jwtOptions.Scheme.Length + 1)
            {
                if (tokenValidAttribute != null)
                {
                    return true;
                }
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
                return false;
            }
            var token = authorizationToken.Substring(jwtOptions.Scheme.Length + 1);
            var (validResult, principal) = await ValidTokenAsync(context, httpContext, provider, token);
            if (!validResult)
                return false;

            if (tokenValidAttribute != null)
            {
                return true;
            }

            httpContext.User = principal;
            var userId = httpContext.User?.Claims.FirstOrDefault(c => c.Type.Equals(nameof(PermissionTokenEntity.UserId), StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (userId.IsNullOrEmpty())
            {
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreateNotLoginResponse(new AuthorizationResponseContext(context)).Value);
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
            var roleIds = httpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(nameof(PermissionTokenEntity.RoleIds), StringComparison.InvariantCultureIgnoreCase))?.Value;
            var secutiryPermissionService = services.GetRequiredService<IPermissionService<TPermissionKey>>();
            if (roleIds.IsNullOrEmpty())
            {
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreatePermissionNotAllowResponse(new AuthorizationResponseContext(context)).Value);
                return false;
            }

            var store = services.GetRequiredService<ICacheStore>();
            var entry = CacheEntryCollection.GetPermissionEntry(userId);
            IEnumerable<string> permissionList = await store.GetAsync<IEnumerable<string>>(entry);
            if (permissionList == null || !permissionList.Any())
            {
                permissionList = await secutiryPermissionService.GetPermissionCodeAsync(roleIds);
                if (permissionList != null && permissionList.Any())
                {
                    await store.SetAsync(entry, permissionList);
                }
            }

            var area = httpContext.GetRouteValue("area")?.ToString();
            var controller = httpContext.GetRouteValue("controller")?.ToString();
            var action = httpContext.GetRouteValue("action")?.ToString();
            var authCode = authCodeAttribute.AuthCode ??
                (area != null ? $"{area}_{controller}.{action}" : $"{controller}.{action}");
            if (!permissionList.Any(d => string.Equals(d, authCode, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreatePermissionNotAllowResponse(new AuthorizationResponseContext(context)).Value);
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
            var logger = services.GetRequiredService<ILogger<RyeDefaultPolicyAuthorizationHandler<TPermissionKey>>>();

            try
            {
                principal = await tokenService.ValidateTokenAsync(JwtTokenType.AccessToken, token);
            }
            catch (SecurityTokenInvalidLifetimeException lifetimeEx)
            {
                logger.LogError(lifetimeEx.ToString());
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreateTokenExpireResponse(new AuthorizationResponseContext(context)).Value);
                return (false, principal);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                await WriteResponseAsync(httpContext, provider.CreateTokenErrorResponse(new AuthorizationResponseContext(context)).Value);
                return (false, principal);
            }
            var tokenTypeStr = principal.Claims.FirstOrDefault(c => c.Type.Equals("TokenType", StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (tokenTypeStr.IsNullOrEmpty() || tokenTypeStr.ParseByInt() != (int)JwtTokenType.AccessToken)
            {
                logger.LogError("JwtTokenType error");
                SetHttpStatusCode(httpContext, HttpStatusCode.OK);
                await WriteResponseAsync(httpContext, provider.CreateTokenErrorResponse(new AuthorizationResponseContext(context)).Value);
                return (false, principal);
            }

            return (true, principal);
        }

        private static void SetHttpStatusCode(HttpContext httpContext, HttpStatusCode statusCode)
        {
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = (int)statusCode;
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, object value)
        {
            // 以JSON格式返回数据
            context.Response.Headers["Content-type"] = "application/json";

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
