using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Rye.Jwt;
using Rye.Jwt.Options;
using Rye.Web;

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
    public class RyeDefaultPolicyAuthorizationHandler : RyePolicyAuthorizationHandler
    {
        /// <summary>
        /// 根据用户角色进行权限认证
        /// </summary>
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
        protected override async Task<bool> AuthorizeCoreAsync(HttpContext httpContext)
        {
            var endpointMetadata = httpContext.Features.Get<IEndpointFeature>()?.Endpoint.Metadata;
            var allowAnonymousAttribute = endpointMetadata.GetMetadata<AllowAnonymousAttribute>();
            if (allowAnonymousAttribute != null)
            {
                return true;
            }
            var jwtTokenService = httpContext.RequestServices.GetRequiredService<IJwtTokenService>();
            var jwtOptions = jwtTokenService.GetOptions(httpContext.Request.GetString("appKey"));
            // 尝试刷新Token
            await TryRefreshTokenAsync(httpContext, jwtTokenService, jwtOptions);

            var authAttributeList = endpointMetadata.GetOrderedMetadata<AuthAttribute>();

            if (authAttributeList == null || !authAttributeList.Any())
                return true;


            var validResult = new TokenValidResult();
            var token = httpContext.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                validResult.HasToken = true;
                validResult.Success = false;
            }
            var (success, result) = await ValidTokenAsync(httpContext, jwtTokenService, jwtOptions, authAttributeList, validResult, token);
            if (success)
            {
                return true;
            }

            SetHttpStatusCode(httpContext, HttpStatusCode.OK);
            await WriteResponseAsync(httpContext, result);
            return false;
        }

        private static async Task<(bool, JsonResult)> ValidTokenAsync(
            HttpContext httpContext,
            IJwtTokenService jwtTokenService,
            JwtOptions jwtOptions,
            IEnumerable<AuthAttribute> attributes,
            TokenValidResult validResult,
            string token)
        {
            ClaimsPrincipal principal = null;
            var services = httpContext.RequestServices;

            var logger = services.GetRequiredService<ILogger<RyeDefaultPolicyAuthorizationHandler>>();

            if (validResult.HasToken)
            {
                try
                {
                    principal = await jwtTokenService.ValidateTokenAsync(JwtTokenType.AccessToken, token, jwtOptions);
                    validResult.Success = true;
                    validResult.HasExpire = false;
                    httpContext.User = principal;
                }
                catch (SecurityTokenInvalidLifetimeException lifetimeEx)
                {
                    logger.LogError(lifetimeEx.ToString());
                    validResult.Success = false;
                    validResult.HasExpire = true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                    validResult.Success = false;
                    validResult.HasExpire = false;
                }
            }

            var orderByAttributes = attributes.GroupBy(d => d.Priority).OrderBy(d => d.Key);
            foreach (var groupAttr in orderByAttributes)
            {
                var success = true;
                JsonResult result = null;
                foreach (var attr in groupAttr)
                {
                    (success, result) = await attr.AuthorizeAsync(httpContext, validResult);
                    if (!success)
                        return (success, result);
                }

                if (success) // 同级别的都验证成功的话，则直接返回结果
                    return (success, result);
            }

            return (true, null);
        }

        private static async Task TryRefreshTokenAsync(HttpContext httpContext, IJwtTokenService jwtTokenService, JwtOptions jwtOptions)
        {
            var refreshToken = httpContext.GetRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

            try
            {
                //var principal = await jwtTokenService.ValidateTokenAsync(JwtTokenType.RefreshToken, refreshToken, jwtOptions);
                var jwtToken = await jwtTokenService.RefreshTokenAsync(refreshToken, jwtOptions);
                httpContext.Response.Headers.Add("access-token", jwtToken.AccessToken);
                httpContext.Response.Headers.Add("x-access-token", jwtToken.RefreshToken);
                httpContext.Response.Headers.Add("access-token-exp", jwtToken.AccessExpires.ToString());
                httpContext.Response.Headers.Add("x-access-token-exp", jwtToken.RefreshExpires.ToString());
            }
            catch
            {
            }
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
