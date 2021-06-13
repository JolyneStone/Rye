using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Rye.Logger;
using Rye.Web.Options;
using Rye.Web.ResponseProvider.ModeValidationAttr;

using System;
using System.Linq;
using System.Net;

namespace Rye.Web
{
    /// <summary>
    /// 模型校验过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ModelValidationAttribute : Attribute, IActionFilter, IOrderedFilter
    {
        public int Order => 0;

        private static IModeValidationResponseProvider _provider;

        protected static IModeValidationResponseProvider Provider
        {
            get
            {
                if (_provider != null)
                    return _provider;

                _provider = App.GetRequiredService<IOptions<RyeWebOptions>>().Value.ModeValidation.Provider;
                return _provider;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var modelState = actionContext.ModelState;
            var logName = $"{actionContext.HttpContext.Request.Path.Value.Trim('/').Replace("/", "_")}_req";

            var logger = actionContext.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(logName);
            if (!modelState.IsValid)
            {
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        string error = GetErrorMessage(actionContext.HttpContext, state.Errors);
                        logger.LogError($"errorMsg:{error}，ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
                        actionContext.Result = Provider.CreateResponse(new ModeValidationContext(actionContext.HttpContext, modelState, key, state.Errors));
                        actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }
                }
            }

            if (actionContext.ActionArguments != null)
                logger.LogInformation($"ActionArguments:{actionContext.ActionArguments.ToJsonString()}");
        }

        protected virtual string GetErrorMessage(HttpContext httpContext, ModelErrorCollection errors)
        {
            return errors.First().ErrorMessage;
        }
    }
}
