using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monica.Logger;
using Monica.Web.Options;
using Monica.Web.ResponseProvider.ModeValidationAttr;
using System;
using System.Linq;
using System.Net;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 模型校验过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        private static IModeValidationResponseProvider _provider;
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var modelState = actionContext.ModelState;
            //var action = actionContext.RouteData.Values["action"];
            //var controller = actionContext.RouteData.Values["controller"];
            var logName = $"{actionContext.HttpContext.Request.Path.Value.Trim('/').Replace("/", "_")}_req";

            if (!modelState.IsValid)
            {
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        string error = state.Errors.First().ErrorMessage;
                        LogRecord.Error(logName, $"errorMsg:{error}，ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
                        if (_provider == null)
                        {
                            _provider = actionContext.HttpContext.RequestServices.GetRequiredService<IOptions<MonicaWebOptions>>().Value.ModeValidation.Provider;
                        }
                        actionContext.Result = _provider.CreateResponse(new ModeValidationContext(actionContext.HttpContext, modelState, state.Errors));
                        actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }
                }
            }

            LogRecord.Info(logName, $"ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
        }
    }
}
