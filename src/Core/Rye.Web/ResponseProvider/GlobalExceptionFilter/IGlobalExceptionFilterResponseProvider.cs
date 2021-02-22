using Microsoft.AspNetCore.Mvc;

namespace Rye.Web.ResponseProvider
{
    public interface IGlobalExceptionFilterResponseProvider
    {
        IActionResult CreateResponse(GlobalExceptionFilterConetxt context);
    }
}
