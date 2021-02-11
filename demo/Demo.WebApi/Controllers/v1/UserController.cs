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

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [ModelValidation]
    [Authorize(policy: "MonicaPermission")]
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
                RoleIds = "1,2"
            };
            IJwtTokenService service = HttpContext.RequestServices.GetService<IJwtTokenService>();
            var token = service.CreateToken(entry, Monica.Enums.ClientType.Browser);
            //var jwtOptions = HttpContext.RequestServices.GetRequiredService<IOptions<JwtOptions>>().Value;
            //var principal = service.ValidateToken(token.AccessToken);
            //await HttpContext.SignInAsync(jwtOptions.Scheme, principal);
            return ApiResult<JsonWebToken>.Create((int)Demo.Core.Common.Enums.StatusCode.Success, token, "成功");
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }
    }
}
