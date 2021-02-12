using Demo.Core.Common;
using Demo.Core.Common.Enums;
using Demo.DataAccess.EFCore.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Monica.Jwt;
using Monica.Web.Filter;

using System.Security.Claims;
using System.Threading.Tasks;
using Monica.Authorization.Abstraction;
using Monica.Authorization.Entities;
using System.Collections.Generic;
using Monica.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Monica.Jwt.Options;
using Microsoft.Extensions.Options;
using Monica.Authorization.Abstraction.Attributes;
using Monica.Enums;
using Demo.Core.Common.Enums;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [ModelValidation]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ILogger<UserController> logger,
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
