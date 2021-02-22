using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rye.Logger;
using System;

namespace Rye.Web.Filter
{
    /// <summary>
    /// 全局接口过滤器
    /// </summary>
    public class GlobalActionFilter : IActionFilter
    {
        private readonly string actionPerf = "ActionExecuting_Millisecond";
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //开始
            if (!context.HttpContext.Items.ContainsKey(actionPerf))
            {
                context.HttpContext.Items[actionPerf] = DateTime.Now.Ticks / 10000d;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //结束
            context.HttpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
            string logMessage = string.Empty;
            #region 记录返回结果和响应时间
            if (context.Exception != null)
            {
                logMessage = "Global Action Error: " + context.Exception.ToString();
            }
            else
            {
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
                        logMessage = "Serialization Failed: " + ex.Message;
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
                        logMessage = "Serialization Failed: " + ex.Message;
                    }
                }
            }

            double elapsedMilliseconds = 0d;
            if (context.HttpContext.Items.ContainsKey(actionPerf))
            {
                double begin = (double)context.HttpContext.Items[actionPerf];
                elapsedMilliseconds = DateTime.Now.Ticks / 10000d - begin;
            }
            logMessage += $"{elapsedMilliseconds} ms";

            //var action = context.RouteData.Values["action"];
            LogRecord.Info(context.HttpContext.Request.Path.Value.Trim('/').Replace("/", "_") + "_rsp", "Response:" + logMessage);

            if (elapsedMilliseconds >= 100d)
            {
                LogRecord.Warn("slow", $"{context.ActionDescriptor.DisplayName}:{elapsedMilliseconds} ms");
            }
            #endregion
        }
    }
}
