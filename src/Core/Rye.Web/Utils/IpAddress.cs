using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rye
{
    public static class IpAddress
    {
        private static readonly Regex Regex = new Regex(@"^(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))$");

        /// <summary>
        /// 获取远程客户端请求的IP地址
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetRemoteIpV4Address(HttpContext httpContext)
        {
            HashSet<string> clentIpSets = new HashSet<string>();
            string[] remoteIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString().Split(',');    //替换成IPV4的格式
            if (remoteIpAddress != null)
            {
                foreach (string remoteIpAddres in remoteIpAddress)
                {
                    string ip = remoteIpAddres;
                    if (string.IsNullOrEmpty(ip))
                    {
                        continue;
                    }
                    if (remoteIpAddres == "::1")
                    {
                        ip = "127.0.0.1";
                    }
                    if (!Regex.Match(ip).Success)
                    {
                        continue;
                    }
                    ip = ip.Trim().Replace(" ", "").Replace(":", "").Trim();
                    clentIpSets.Add(ip);
                }
            }

            string[] clentIps = httpContext.Request.Headers["X-Forwarded-For"].ToArray();
            if (clentIps != null)
            {
                foreach (string clentIp in clentIps)
                {
                    if (string.IsNullOrEmpty(clentIp))
                    {
                        continue;
                    }

                    remoteIpAddress = clentIp.Trim().Replace(" ", "").Split(',');
                    foreach (string remoteIpAddres in remoteIpAddress)
                    {
                        string ip = remoteIpAddres;
                        if (string.IsNullOrEmpty(ip))
                        {
                            continue;
                        }
                        if (remoteIpAddres == "::1")
                        {
                            ip = "127.0.0.1";
                        }
                        if (!Regex.Match(ip).Success)
                        {
                            continue;
                        }
                        ip = ip.Trim().Replace(" ", "").Replace(":", "").Trim();
                        clentIpSets.Add(ip);
                    }
                }
            }
            return clentIpSets;
        }

        /// <summary>
        /// 获取远程客户端请求的IP地址,返回多个IP以逗号隔开
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteIpV4AddressStr(HttpContext httpContext)
        {
            HashSet<string> ips = GetRemoteIpV4Address(httpContext);
            if (ips != null && ips.Count > 0)
            {
                return string.Join(",", ips);
            }

            return string.Empty;
        }
    }
}
