using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Monica.Logger;
using Monica.Web.Options;
using Monica.Web.ResponseProvider;
using System;
using System.IO;
using System.Net;

namespace Monica.Web.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private static IGlobalExceptionFilterResponseProvider _provider;
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

                LogRecord.Error("GlobalException", $@"Error:{context.Exception}{Environment.NewLine}
                    Headers:{context.HttpContext.Request.Headers.ToJsonString()}{Environment.NewLine}
                    Query:{context.HttpContext.Request.Query.ToJsonString()}{Environment.NewLine}
                    Body:{requestBody}");

                if (_provider == null)
                {
                    _provider = context.HttpContext.RequestServices.GetRequiredService<IOptions<MonicaWebOptions>>().Value.GlobalExceptionFilter.Provider;
                }
                context.Result = _provider.CreateResponse(new GlobalExceptionFilterConetxt(context.HttpContext, context.Exception));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.ExceptionHandled = true;
            }
            catch (Exception ex)
            {
                LogRecord.Error("GlobalExceptionFilter", $@"Error:{context.Exception}{Environment.NewLine}
                    Headers:{context.HttpContext.Request.Headers.ToJsonString()}{Environment.NewLine}
                    Query:{context.HttpContext.Request.Query.ToJsonString()}{Environment.NewLine}
                    Exception:{ex.ToString()}");
            }


        }
    }
}
