using Demo.Common;
using Demo.Common.Enums;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Rye.Business.Validate;
using Rye.Security;
using Rye.Web;

using System.Drawing;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [AllowAnonymous]
    public class CommonController : BaseController
    {
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<string>> VerifyCode()
        {
            var verifyCodeService = HttpContext.RequestServices.GetService<IVerifyCodeService>();
            ValidateCoder coder = new ValidateCoder()
            {
                RandomColor = true,
                RandomItalic = true,
                RandomLineCount = 7,
                RandomPointPercent = 10,
                RandomPosition = true
            };
            Bitmap bitmap = coder.CreateImage(4, out string code);
            var id = await verifyCodeService.SetCodeAsync(code);
            return Result<string>(DefaultStatusCode.Success, verifyCodeService.GetImageString(bitmap, id));
        }

        [HttpPost]
        public async Task<ApiResult<string>> Encrypt([FromForm] string param, [FromForm] int appId,
            [FromServices] ISecurityService securityService)
        {
            return Result<string>(DefaultStatusCode.Success, await securityService.EncryptAsync(appId, param));
        }

        [HttpPost]
        public async Task<ApiResult<string>> Decrypt([FromForm] string param, [FromForm] int appId,
                [FromServices] ISecurityService securityService)
        {
            return Result<string>(DefaultStatusCode.Success, await securityService.DecryptAsync(appId, param));
        }

        [Security(decryptRequestBody: true, encryptResponseBody: true)]
        [HttpPost]
        public string SecurityTest([FromForm]string val)
        {
            return val;
        }
    }
}
