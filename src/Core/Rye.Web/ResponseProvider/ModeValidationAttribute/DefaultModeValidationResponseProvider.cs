using Microsoft.AspNetCore.Mvc;
using Rye.Web.Internal;
using System.Linq;
using System.Net;

namespace Rye.Web.ResponseProvider.ModeValidationAttr
{
    public class DefaultModeValidationResponseProvider : IModeValidationResponseProvider
    {
        public IActionResult CreateResponse(ModeValidationContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.BadRequest, $"Parameters invalid: {context.Errors.FirstOrDefault()?.ErrorMessage}"));
        }
    }
}
