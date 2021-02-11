using Microsoft.AspNetCore.Mvc;

namespace Monica.Web.ResponseProvider.ModeValidationAttr
{
    public interface IModeValidationResponseProvider
    {
        IActionResult CreateResponse(ModeValidationContext context);
    }
}
