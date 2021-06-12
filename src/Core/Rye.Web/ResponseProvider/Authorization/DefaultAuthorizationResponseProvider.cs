using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rye.Web.Internal;
using System.Net;

namespace Rye.Web.ResponseProvider.Authorization
{
    public class DefaultAuthorizationResponseProvider : IAuthorizationResponseProvider
    {
        public JsonResult CreateNotLoginResponse(HttpContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, I18n.GetText(LangKeyEnum.NotLogin)));
        }

        public JsonResult CreateTokenExpireResponse(HttpContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, I18n.GetText(LangKeyEnum.TokenExpire)));
        }

        public JsonResult CreatePermissionNotAllowResponse(HttpContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, I18n.GetText(LangKeyEnum.PermissionNotAllow)));
        }

        public JsonResult CreateTokenErrorResponse(HttpContext context)
        {
            return new JsonResult(Result
             .Create(HttpStatusCode.Unauthorized, I18n.GetText(LangKeyEnum.TokenError)));
        }
    }
}
