using Microsoft.AspNetCore.Mvc;

namespace Rye.Web.ResponseProvider
{
    public interface IGlobalActionFilterResponseProvider
    {
        IActionResult CreateResponse(GlobalActionFilterConetxt context);
    }
}
