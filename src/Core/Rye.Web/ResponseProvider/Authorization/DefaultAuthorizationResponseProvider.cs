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
                .Create(HttpStatusCode.Unauthorized, $"Please log in and try again"));
        }

        public JsonResult CreateTokenExpireResponse(HttpContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, $"Access token has expired"));
        }

        public JsonResult CreatePermissionNotAllowResponse(HttpContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, $"You do not have access to this api"));
        }

        public JsonResult CreateTokenErrorResponse(HttpContext context)
        {
            return new JsonResult(Result
             .Create(HttpStatusCode.Unauthorized, $"Access token is error"));
        }
    }
}
