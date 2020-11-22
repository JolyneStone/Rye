using Microsoft.AspNetCore.Mvc;

using Monica.Web.Internal;

using System.Net;

namespace Monica.Web.ResponseProvider.AvoidRepeatableRequestAttr
{
    public class DefaultAvoidRepeatableRequestResponseProvider : IAvoidRepeatableRequestResponseProvider
    {
        public IActionResult CreateResponse(AvoidRepeatableRequestContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Forbidden, "You can't repeat the request, please try again later!"));
        }
    }
}
