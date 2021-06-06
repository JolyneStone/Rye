using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Net;
using Rye.Web.ResponseProvider;
using Rye.Web.Utils;
using Microsoft.Extensions.Options;
using Rye.Web.Options;

namespace Rye.Web.Filter
{
    public class GlobalActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly string actionPerf = "ActionExecuting_Millisecond";
        private static IGlobalActionFilterResponseProvider _provider;

        protected static IGlobalActionFilterResponseProvider Provider
        {
            get
            {
                if (_provider != null)
                    return _provider;

                _provider = App.GetRequiredService<IOptions<RyeWebOptions>>().Value.GlobalActionFilter.Provider;
                return _provider;
            }
        }

        public int Order => -9999;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var logName = httpContext.Request.Path.Value.Trim('/').Replace("/", "_");
            var loggerFactory = httpContext.RequestServices.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(logName + "_req");

            //开始
            if (!context.HttpContext.Items.ContainsKey(actionPerf))
            {
                context.HttpContext.Items[actionPerf] = DateTimeOffset.UtcNow.Ticks / 10000d;
            }

            // 获取请求的 Url 地址
            var requestUrl = httpContext.Request.GetRequestUrlAddress();

            logger.LogInformation($"url: {requestUrl}, params: {context.ActionArguments.ToJsonString()}");

            var executedContext = await next();
            logger = loggerFactory.CreateLogger(logName + "_rsp");
            try
            {
                if (executedContext.Exception != null)
                {
                    executedContext.Result = OnActionException(httpContext, logger, executedContext.Exception);
                    executedContext.ExceptionHandled = true;
                }
                else
                {
                    OnActionExecuted(context, httpContext, logger);
                }
            }
            catch (Exception exception)
            {
                executedContext.Result = OnActionException(httpContext, logger, exception);
                executedContext.ExceptionHandled = true;
            }
        }


        private void OnActionExecuted(ActionExecutingContext context, HttpContext httpContext, ILogger logger)
        {
            //结束
            //context.HttpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
            string logMessage = string.Empty;
            #region 记录返回结果和响应时间
            if (context.Result is JsonResult jsonResult)
            {
                try
                {
                    if (jsonResult != null)
                    {
                        logMessage = jsonResult.Value?.ToJsonString();
                    }
                }
                catch (Exception ex)
                {
                    logMessage = "serialization failed: " + ex.Message;
                }
            }
            else if (context.Result is ObjectResult objectResult)
            {
                try
                {
                    if (objectResult != null)
                    {
                        logMessage = objectResult.Value?.ToJsonString();
                    }
                }
                catch (Exception ex)
                {
                    logMessage = "ferialization failed: " + ex.Message;
                }
            }

            if (!string.IsNullOrEmpty(logMessage)) // 只记录返回JsonResult、ObjectResult和异常请求的返回信息
            {
                double elapsedMilliseconds = 0d;
                if (context.HttpContext.Items.ContainsKey(actionPerf))
                {
                    double begin = (double)context.HttpContext.Items[actionPerf];
                    elapsedMilliseconds = DateTimeOffset.UtcNow.Ticks / 10000d - begin;

                    logMessage += $"{elapsedMilliseconds} ms";

                    if (elapsedMilliseconds >= 100d)
                    {
                        logger.LogWarning("slow", $"{context.ActionDescriptor.DisplayName}:{elapsedMilliseconds} ms");
                    }
                }

                logger.LogInformation(logMessage);
            }

            #endregion
        }

        private IActionResult OnActionException(HttpContext httpContext, ILogger logger, Exception exception)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            logger.LogError($@"exception:{exception}");

            return Provider.CreateResponse(new GlobalActionFilterConetxt(httpContext, exception));
        }
    }
}
