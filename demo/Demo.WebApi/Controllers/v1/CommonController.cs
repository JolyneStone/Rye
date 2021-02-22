using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Rye.Business.Validate;
using Rye.Security;

using System.Drawing;

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
        public string VerifyCode()
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
            verifyCodeService.SetCode(code, out string id);
            return verifyCodeService.GetImageString(bitmap, id);
        }

        [HttpPost]
        public string Encrypt([FromForm] string param, [FromForm] int appId,
            [FromServices] ISecurityService securityService)
        {
            return securityService.Encrypt(appId, param);
        }

        [HttpPost]
        public string Decrypt([FromForm] string param, [FromForm] int appId,
                [FromServices] ISecurityService securityService)
        {
            return securityService.Decrypt(appId, param);
        }
    }
}
