using Microsoft.AspNetCore.Mvc;
using Monica.Web.Internal;
using System.Net;

namespace Monica.Web.ResponseProvider.Authorization
{
    public class DefaultAuthorizationResponseProvider : IAuthorizationResponseProvider
    {
        public JsonResult CreateNotLoginResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Forbidden, $"Please log in and try again"));
        }

        public JsonResult CreateTokenExpireResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Forbidden, $"Access token has expired"));
        }

        public JsonResult CreatePermissionNotAllowResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Forbidden, $"You do not have access to this api"));
        }

        public JsonResult CreateTokenErrorResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
             .Create(HttpStatusCode.Forbidden, $"Access token is error"));
        }
    }
}
