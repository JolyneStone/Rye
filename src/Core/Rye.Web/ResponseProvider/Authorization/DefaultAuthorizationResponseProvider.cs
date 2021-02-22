using Microsoft.AspNetCore.Mvc;
using Rye.Web.Internal;
using System.Net;

namespace Rye.Web.ResponseProvider.Authorization
{
    public class DefaultAuthorizationResponseProvider : IAuthorizationResponseProvider
    {
        public JsonResult CreateNotLoginResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, $"Please log in and try again"));
        }

        public JsonResult CreateTokenExpireResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, $"Access token has expired"));
        }

        public JsonResult CreatePermissionNotAllowResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.Unauthorized, $"You do not have access to this api"));
        }

        public JsonResult CreateTokenErrorResponse(AuthorizationResponseContext context)
        {
            return new JsonResult(Result
             .Create(HttpStatusCode.Unauthorized, $"Access token is error"));
        }
    }
}
