using Microsoft.AspNetCore.Http;
using System;

namespace Rye.Web.ResponseProvider
{
    public class GlobalExceptionFilterConetxt
    {
        public GlobalExceptionFilterConetxt(HttpContext httpContext, Exception exception)
        {
            HttpContext = httpContext;
            Exception = exception;
        }

        public HttpContext HttpContext { get; }
        public Exception Exception { get; }
    }
}
