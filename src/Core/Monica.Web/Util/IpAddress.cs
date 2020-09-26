using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Monica.Web.Util
{
    public class IpAddress
    {
        private static readonly Regex Regex = new Regex(@"^(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))$");
        private readonly HttpContext _httpContext;
        public IpAddress(IHttpContextAccessor accessor)
        {
            _httpContext = accessor.HttpContext;
        }
        /// <summary>
        /// 获取远程客户端请求的IP地址
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetRemoteIpV4Address()
        {
            HashSet<string> clentIpSets = new HashSet<string>();
            string[] remoteIpAddress = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString().Split(',');    //替换成IPV4的格式
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

            string[] clentIps = _httpContext.Request.Headers["X-Forwarded-For"].ToArray();
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
        public string GetRemoteIpV4AddressStr()
        {
            HashSet<string> ips = GetRemoteIpV4Address();
            if (ips != null && ips.Count > 0)
            {
                return string.Join(",", ips);
            }

            return string.Empty;
        }
    }
}
