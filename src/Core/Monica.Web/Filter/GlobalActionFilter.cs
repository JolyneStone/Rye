using Microsoft.AspNetCore.Mvc.Filters;
using Monica.Logger;
using System;

namespace Monica.Web.Filter
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
            #region 记录返回结果和响应时间
            dynamic actionResult = context.Result?.GetType().Name == "EmptyResult" ? new { Value = "无返回结果" } : context.Result as dynamic;
            string result = "在返回结果前发生了异常" + context.Exception?.Message;
            try
            {
                if (actionResult != null)
                {
                    result = actionResult.Value.ToJsonString();
                }
            }
            catch (Exception ex)
            {
                result = "日志未获取到结果，返回的数据无法序列化" + ex.Message;
            }

            double elapsedMilliseconds = 0d;
            if (context.HttpContext.Items.ContainsKey(actionPerf))
            {
                double begin = (double)context.HttpContext.Items[actionPerf];
                elapsedMilliseconds = DateTime.Now.Ticks / 10000d - begin;
            }
            result += $"{elapsedMilliseconds} ms";

            var action = context.RouteData.Values["action"];
            LogRecord.Info(action + "_res", "Response:" + result);

            if (elapsedMilliseconds >= 100d)
            {
                LogRecord.Warn("slow", $"{context.ActionDescriptor.DisplayName}:{elapsedMilliseconds} ms");
            }
            #endregion
        }
    }
}
