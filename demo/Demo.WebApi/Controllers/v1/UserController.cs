using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monica.Web.Filter;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [ModelValidation]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }
    }
}
