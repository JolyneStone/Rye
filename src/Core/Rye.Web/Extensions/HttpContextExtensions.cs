using Microsoft.AspNetCore.Http;

using System;
using System.Linq;
using System.Text;

namespace Rye.Web
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paramName">参数名</param>
        /// <returns></returns>
        public static string GetString(this HttpRequest request, string paramName)
        {
            var val = request.Headers[paramName];
            if (val.Count <= 0)
            {
                val = request.Query[paramName];
            }
            if (val.Count <= 0 && request.Form != null)
            {
                request.Form.TryGetValue(paramName, out val);
            }

            return val.ToString();
        }

        /// <summary>
        /// 获取用户声明的值
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetClaimValue(this HttpContext httpContext, string type)
        {
            if (httpContext == null || httpContext.User == null || httpContext.User.Claims == null)
                return default;

            return httpContext.User.Claims.FirstOrDefault(d =>
                        d.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        /// <summary>
        /// 获取本机 IPv4地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetLocalIpAddressToIPv4(this HttpContext context)
        {
            return context.Connection.LocalIpAddress?.MapToIPv4()?.ToString();
        }

        /// <summary>
        /// 获取本机 IPv6地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetLocalIpAddressToIPv6(this HttpContext context)
        {
            return context.Connection.LocalIpAddress?.MapToIPv6()?.ToString();
        }

        /// <summary>
        /// 获取远程 IPv4地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRemoteIpAddressToIPv4(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
        }

        /// <summary>
        /// 获取远程 IPv6地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRemoteIpAddressToIPv6(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
        }

        /// <summary>
        /// 获取完整请求地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestUrlAddress(this HttpRequest request)
        {
            return new StringBuilder()
                    .Append(request.Scheme)
                    .Append("://")
                    .Append(request.Host)
                    .Append(request.PathBase)
                    .Append(request.Path)
                    .Append(request.QueryString)
                    .ToString();
        }

        /// <summary>
        /// 获取来源地址
        /// </summary>
        /// <param name="request"></param>
        /// <param name="refererHeaderKey"></param>
        /// <returns></returns>
        public static string GetRefererUrlAddress(this HttpRequest request, string refererHeaderKey = "Referer")
        {
            return request.Headers[refererHeaderKey].ToString();
        }
    }
}
