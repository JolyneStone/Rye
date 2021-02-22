using Microsoft.AspNetCore.Mvc;

namespace Rye.Web.ResponseProvider.AvoidRepeatableRequestAttr
{
    public interface IAvoidRepeatableRequestResponseProvider
    {
        IActionResult CreateResponse(AvoidRepeatableRequestContext context);
    }
}
