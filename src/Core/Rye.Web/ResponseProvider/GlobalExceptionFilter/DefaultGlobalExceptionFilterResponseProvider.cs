using Microsoft.AspNetCore.Mvc;
using Rye.Web.Internal;
using System.Net;

namespace Rye.Web.ResponseProvider
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
