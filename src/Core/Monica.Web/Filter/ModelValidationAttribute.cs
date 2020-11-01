using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Monica.Logger;
using Monica.Web.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Monica.Web.Internal;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 模型校验过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var modelState = actionContext.ModelState;
            var action = actionContext.RouteData.Values["action"];
            var controller = actionContext.RouteData.Values["controller"];
            var logName = $"{controller}_{action}_param";

            if (!modelState.IsValid)
            {
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        string error = state.Errors.First().ErrorMessage;
                        LogRecord.Error(logName, $"errorMsg:{error}，ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
                        var options = actionContext.HttpContext.RequestServices.GetService<IOptions<MonicaWebOptions>>().Value;
                        Dictionary<string, object> result;
                        if(options == null || options.Response== null)
                        {
                            result = null;
                        }
                        else
                        {
                            result = options.Response.ParametersInvalid(error, null);
                        }
                        actionContext.Result = new BadRequestObjectResult(modelState) { Value = result };
                        return;
                    }
                }
            }

            LogRecord.Info(logName, $"ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
        }
    }
}
