using Demo.Core.Common;
using Demo.Core.Common.Enums;
using Demo.Core.Model.Input;
using Demo.Library.Abstraction;
using Demo.Library.Dto;
using Demo.WebApi.Model.Intput;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Rye;
using Rye.Authorization.Abstraction.Attributes;
using Rye.Business.Validate;
using Rye.Entities.Abstractions;
using Rye.Jwt;
using Rye.Web.Filter;

using System;
using System.Linq;
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
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public BasicUserController(ILogger<BasicUserController> logger,
            IUserService userService,
            IJwtTokenService jwtTokenService)
        {
            _logger = logger;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="basicInput"></param>
        /// <param name="input"></param>
        /// <param name="loginService"></param>
        /// <param name="verifyCodeService"></param>
        /// <param name="jwtTokenService"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<JsonWebToken>> Login([FromQuery] BasicInput basicInput, [FromBody] LoginInput input,
            [FromServices] ILoginService loginService)
        {
            //var validResult = verifyCodeService.CheckCode(input.VerifyCodeId, input.VerifyCode, false);
            //if (!validResult)
            //{
            //    return Result<JsonWebToken>(CommonStatusCode.VerifyCodeError);
            //}

            var (code, userInfo) = await loginService.LoginAsync(
                basicInput.AppKey,
                input.Account.FromBase64String(),
                input.Password.FromBase64String());
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
            var token = await _jwtTokenService.CreateTokenAsync(entry);
            return Result(CommonStatusCode.Success, token);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="basicInput"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResult> Logout([FromQuery] BasicInput basicInput, [FromQuery] string token)
        {
            if(token.IsNullOrEmpty())
                return Result(CommonStatusCode.Success);

            var principal = await _jwtTokenService.ValidateTokenAsync(JwtTokenType.AccessToken, token);
            var userIdStr = principal.Claims.FirstOrDefault(d => d.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
            var clientType = basicInput.ClientType.ToString();

            await _jwtTokenService.DeleteTokenAsync(userIdStr, clientType);
            return Result(CommonStatusCode.Success);
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="basicInput"></param>
        /// <param name="refreshToken"></param>
        /// <param name="tokenService"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<JsonWebToken>> RefreshToken([FromQuery] BasicInput basicInput, [FromQuery] string refreshToken)
        {
            if (refreshToken.IsNullOrEmpty())
            {
                return Result<JsonWebToken>(CommonStatusCode.ParametersError);
            }

            ClaimsPrincipal principal;
            try
            {
                principal = await _jwtTokenService.ValidateTokenAsync(JwtTokenType.RefreshToken, refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(RefreshToken)}: Exception: {ex.ToString()}");
                return Result<JsonWebToken>(CommonStatusCode.Fail);
            }

            var token = await _jwtTokenService.RefreshTokenAsync(refreshToken);
            return Result(CommonStatusCode.Success, token);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<UserInfoDto>> GetUserInfo([FromQuery] BasicInput basicInput)
        {
            var appIdStr = HttpContext.User.Claims.FirstOrDefault(d => d.Type.Equals("AppId", StringComparison.InvariantCultureIgnoreCase))?.Value;
            var userIdStr = HttpContext.User.Claims.FirstOrDefault(d => d.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (appIdStr.IsNullOrEmpty() || userIdStr.IsNullOrEmpty())
            {
                return Result<UserInfoDto>(CommonStatusCode.UsertokenInvalid);
            }

            return Result(CommonStatusCode.Success,
                await _userService.GetBasicUserAsync(appIdStr.ParseByInt(), userIdStr.ParseByInt()));
        }

    }
}
