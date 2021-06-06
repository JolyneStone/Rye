using Microsoft.AspNetCore.Mvc;
using Rye.Web.Internal;
using System.Net;

namespace Rye.Web.ResponseProvider
{
    public class DefaultGlobalActionFilterResponseProvider : IGlobalActionFilterResponseProvider
    {
        public IActionResult CreateResponse(GlobalActionFilterConetxt context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.InternalServerError, context.Exception?.Message));
        }
    }
}
