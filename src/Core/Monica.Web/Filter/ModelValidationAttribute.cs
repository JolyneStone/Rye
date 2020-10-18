using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monica.Logger;
using System;
using System.Linq;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 模型校验过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
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
                string error = string.Empty;
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        error = state.Errors.First().ErrorMessage;
                        LogRecord.Error(logName, $"errorMsg:{error}，ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
                        var result = new ApiResult(error, (int)ResultCodeType.ParametersError, ReturnCodeType.BadRequest);
                        actionContext.Result = new BadRequestObjectResult(modelState) { Value = result };
                        return;
                    }
                }
            }

            LogRecord.Info(logName, $"ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
        }
    }
}
