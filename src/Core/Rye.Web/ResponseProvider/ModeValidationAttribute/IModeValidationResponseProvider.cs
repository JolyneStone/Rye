using Microsoft.AspNetCore.Mvc;

namespace Rye.Web.ResponseProvider.ModeValidationAttr
{
    public interface IModeValidationResponseProvider
    {
        IActionResult CreateResponse(ModeValidationContext context);
    }
}
