using Demo.Core.Common;
using Demo.Core.Common.Enums;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Monica.Authorization.Entities;
using Monica.EntityFrameworkCore;
using Monica.Enums;
using Monica.Jwt;
using Monica.Web.Filter;

using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [ModelValidation]
    [Authorize]
    public class BasicUserController : Controller
    {
        private readonly ILogger<BasicUserController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public BasicUserController(ILogger<BasicUserController> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResult<JsonWebToken>> Login()
        {
            //生成Token
            var entry = new PermissionTokenEntity
            {
                AppId = "1",
                UserId = "1",
                RoleIds = "1,2",
                ClientType = ((int)ClientType.Browser).ToString()
            };
            IJwtTokenService service = HttpContext.RequestServices.GetService<IJwtTokenService>();
            var token = await service.CreateTokenAsync(entry);
            return ApiResult<JsonWebToken>.Create((int)CommonStatusCode.Success, token, "成功");
        }

        [HttpGet]
        public string Get()
        {
            var p = HttpContext.User;
            return "Hello World!";
        }
    }
}
