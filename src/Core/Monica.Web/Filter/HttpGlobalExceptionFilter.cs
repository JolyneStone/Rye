using Microsoft.AspNetCore.Mvc.Filters;
using Monica.Logger;
using System;
using System.IO;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            try
            {
                string requestBody = null;
                if (context.HttpContext.Request.Body != null)
                {
                    using (StreamReader streamReader = new StreamReader(context.HttpContext.Request.Body))
                    {
                        requestBody = streamReader.ReadToEnd();
                    }
                }

                LogRecord.Error("HttpGlobal", $@"Error:{context.Exception}{Environment.NewLine}
                    Headers:{context.HttpContext.Request.Headers.ToJsonString()}{Environment.NewLine}
                    Query:{context.HttpContext.Request.Query.ToJsonString()}{Environment.NewLine}
                    Body:{requestBody}");
            }
            catch (Exception ex)
            {
                LogRecord.Error("HttpGlobalExceptionFilter", $@"Error:{context.Exception}{Environment.NewLine}
                    Headers:{context.HttpContext.Request.Headers.ToJsonString()}{Environment.NewLine}
                    Query:{context.HttpContext.Request.Query.ToJsonString()}{Environment.NewLine}
                    Exception:{ex.ToString()}");
            }
        }
    }
}
