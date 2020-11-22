using Microsoft.AspNetCore.Mvc;

namespace Monica.Web.ResponseProvider
{
    public interface IGlobalExceptionFilterResponseProvider
    {
        IActionResult CreateResponse(GlobalExceptionFilterConetxt context);
    }
}
