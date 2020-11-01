using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monica.DependencyInjection;
using Monica.Web.Internal;
using Monica.Web.Options;
using Monica.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 防止重复提交过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AvoidRepeatableCommitAttribute: ActionFilterAttribute
    {
        public AvoidRepeatableCommitAttribute()
        {
            Seconds = 5;
        }

        public AvoidRepeatableCommitAttribute(int seconds)
        {
            Seconds = seconds;
        }

        /// <summary>
        /// 默认为五秒
        /// </summary>
        public int Seconds { get; set; }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var action = actionContext.RouteData.Values["action"];
            var controller = actionContext.RouteData.Values["controller"];
            var ips = IpAddress.GetRemoteIpV4Address(actionContext.HttpContext);
            var key = $"{controller}_{action}_{ips.First()}_{ips.ExpandAndToString().GetHashCode()}"; // 暂时用IP作为
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            if (cache.Exist(key))
            {
                var options = actionContext.HttpContext.RequestServices.GetService<IOptions<MonicaWebOptions>>().Value;
                Dictionary<string, object> result;
                if (options == null || options.Response == null)
                {
                    result = null;
                }
                else
                {
                    result = options.Response.ParametersInvalid("You can't repeat the submission, please try again later!", null);
                }
                actionContext.Result = new BadRequestObjectResult(null) { Value = result };
            }
            else
            {
                cache.Set(key, 1, Seconds);
            }
        }
    }
}
