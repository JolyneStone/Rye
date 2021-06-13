using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Rye
{
    public class WebApp: App
    {
        /// <summary>
        /// 获取Web主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IWebHostEnvironment WebHostEnvironment { get; internal set; }

        /// <summary>
        /// 获取当前HttpContext上下文
        /// </summary>
        public static HttpContext HttpContext { get => HttpContextUtil.GetCurrentHttpContext(); }
    }
}
