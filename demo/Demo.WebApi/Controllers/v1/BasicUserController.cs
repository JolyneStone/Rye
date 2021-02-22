using Demo.Core.Common;
using Demo.Core.Common.Enums;
using Demo.Core.Model.Input;
using Demo.Library.Abstraction;
using Demo.Library.Dto;
using Demo.WebApi.Attribute;
using Demo.WebApi.Model.Intput;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rye;
using Rye.Authorization.Abstraction.Attributes;
using Rye.Business.Validate;
using Rye.Jwt;
using Rye.Web.Attribute;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [Login]
    public class BasicUserController : BaseController
    {
        private readonly ILogger<BasicUserController> _logger;
        private readonly ILoginService _loginService;

        public BasicUserController(ILogger<BasicUserController> logger,
            ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [Security(decryptRequestBody: true, encryptResponseBody: true)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<JsonWebToken>> Login([FromQuery] BasicInput basicInput, [FromForm] LoginInput input,
            [FromServices] IVerifyCodeService verifyCodeService,
            [FromServices] IJwtTokenService jwtTokenService)
        {
            var validResult = verifyCodeService.CheckCode(input.VerifyCode, input.VerifyCodeId, false);
            if (!validResult)
            {
                return Result<JsonWebToken>(CommonStatusCode.VerifyCodeError);
            }

            var (code, userInfo) = await _loginService.LoginAsync(basicInput.AppKey, input.Account, input.Password);
            if (code != CommonStatusCode.Success)
            {
                return Result<JsonWebToken>(code);
            }

            //生成Token
            var entry = new JwtTokenEntity
            {
                AppId = userInfo.AppId.ToString(),
                UserId = userInfo.Id.ToString(),
                RoleIds = string.Join(',', userInfo.RoleIds),
                ClientType = basicInput.ClientType.ToString(),
                Nickname = userInfo.Nickame,
                Email = userInfo.Email,
                Phone = userInfo.Phone
            };
            var token = await jwtTokenService.CreateTokenAsync(entry);
            return Result(CommonStatusCode.Success, token);
        }

        [HttpGet]
        public async Task<ApiResult<JsonWebToken>> RefreshToken([FromQuery]BasicInput basicInput, [FromQuery]string refreshToken,
            [FromServices] IJwtTokenService tokenService)
        {
            if (refreshToken.IsNullOrEmpty())
            {
                return Result<JsonWebToken>(CommonStatusCode.ParametersError);
            }

            ClaimsPrincipal principal;
            try
            {
                principal = await tokenService.ValidateTokenAsync(JwtTokenType.RefreshToken, refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(RefreshToken)}: Exception: {ex.ToString()}");
                return Result<JsonWebToken>(CommonStatusCode.Fail);
            }

            var token = await tokenService.RefreshTokenAsync(refreshToken);
            return Result(CommonStatusCode.Success, token);
        }
    }
}
