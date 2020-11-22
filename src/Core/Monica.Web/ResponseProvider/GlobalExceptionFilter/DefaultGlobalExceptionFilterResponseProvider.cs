using Microsoft.AspNetCore.Mvc;
using Monica.Web.Internal;
using System.Net;

namespace Monica.Web.ResponseProvider
{
    public class DefaultGlobalExceptionFilterResponseProvider : IGlobalExceptionFilterResponseProvider
    {
        public IActionResult CreateResponse(GlobalExceptionFilterConetxt context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.InternalServerError, context.Exception?.Message));
        }
    }
}
