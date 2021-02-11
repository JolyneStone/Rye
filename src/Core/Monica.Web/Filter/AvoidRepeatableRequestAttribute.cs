using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monica.DependencyInjection;
using Monica.Web.Options;
using Monica.Web.ResponseProvider.AvoidRepeatableRequestAttr;
using Monica.Web.Util;
using System;
using System.Linq;
using System.Net;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 防止重复提交
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AvoidRepeatableRequestAttribute : ActionFilterAttribute
    {
        private static IAvoidRepeatableRequestResponseProvider _provider;
        public AvoidRepeatableRequestAttribute()
        {
            Seconds = 5;
        }

        public AvoidRepeatableRequestAttribute(int seconds)
        {
            Seconds = seconds;
        }

        /// <summary>
        /// 默认为五秒
        /// </summary>
        public int Seconds { get; set; }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var request = actionContext.HttpContext.Request;
            //var action = actionContext.RouteData.Values["action"];
            //var controller = actionContext.RouteData.Values["controller"];
            var ips = IpAddress.GetRemoteIpV4Address(actionContext.HttpContext);
            var key = $"{ips.First()}{request.Path}{request.QueryString}";
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            if (cache.Exist(key))
            {
                if (_provider == null)
                {
                    _provider = actionContext.HttpContext.RequestServices.GetRequiredService<IOptions<MonicaWebOptions>>().Value.AvoidRepeatableRequest.Provider;
                }

                actionContext.Result = _provider.CreateResponse(new AvoidRepeatableRequestContext(actionContext.HttpContext, key));
                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                cache.Set(key, 1, Seconds);
            }
        }
    }
}
