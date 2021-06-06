using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.Jwt.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.Web
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取 Access Token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetAccessToken(this HttpContext httpContext)
        {
            return GetTokenCore(httpContext, "Authorization");
        }

        /// <summary>
        /// 获取 Refresh Token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetRefreshToken(this HttpContext httpContext)
        {
            return GetTokenCore(httpContext, "X-Authorization");
        }

        private static string GetTokenCore(HttpContext httpContext, string headKey)
        {
            if (!httpContext.Request.Headers.TryGetValue(headKey, out var authorizationTokenVal))
            {
                return null;
            }
            var authorizationToken = authorizationTokenVal.ToString();
            if (string.IsNullOrEmpty(authorizationToken))
            {
                return null;
            }

            var jwtOptions = httpContext.RequestServices.GetRequiredService<IOptions<JwtOptions>>().Value;
            if (authorizationToken.Length < jwtOptions.Scheme.Length + 1)
            {
                return null;
            }
            var token = authorizationToken.Substring(jwtOptions.Scheme.Length + 1);
            return token;
        }
    }
}
