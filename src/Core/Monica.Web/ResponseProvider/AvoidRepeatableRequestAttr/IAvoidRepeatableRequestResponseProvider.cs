using Microsoft.AspNetCore.Mvc;

namespace Monica.Web.ResponseProvider.AvoidRepeatableRequestAttr
{
    public interface IAvoidRepeatableRequestResponseProvider
    {
        IActionResult CreateResponse(AvoidRepeatableRequestContext context);
    }
}
