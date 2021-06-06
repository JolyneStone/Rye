using Microsoft.AspNetCore.Http;
using System;

namespace Rye.Web.ResponseProvider
{
    public class GlobalActionFilterConetxt
    {
        public GlobalActionFilterConetxt(HttpContext httpContext, Exception exception)
        {
            HttpContext = httpContext;
            Exception = exception;
        }

        public HttpContext HttpContext { get; }
        public Exception Exception { get; }
    }
}
