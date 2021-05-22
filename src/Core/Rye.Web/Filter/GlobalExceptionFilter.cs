using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.Logger;
using Rye.Web.Options;
using Rye.Web.ResponseProvider;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Rye.Web.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private static IGlobalExceptionFilterResponseProvider _provider;
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            try
            {
                string requestBody = null;
                if (context.HttpContext.Request.Body != null)
                {
                    using (StreamReader streamReader = new StreamReader(context.HttpContext.Request.Body))
                    {
                        requestBody = await streamReader.ReadToEndAsync();
                    }
                }

                LogRecord.Error("GlobalException", $@"Error:{context.Exception}{Environment.NewLine}
                    Headers:{context.HttpContext.Request.Headers.ToJsonString()}{Environment.NewLine}
                    Query:{context.HttpContext.Request.Query.ToJsonString()}{Environment.NewLine}
                    Body:{requestBody}");

                if (_provider == null)
                {
                    _provider = context.HttpContext.RequestServices.GetRequiredService<IOptions<RyeWebOptions>>().Value.GlobalExceptionFilter.Provider;
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
