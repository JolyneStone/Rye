using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Monica.Web.ResponseProvider.Authorization
{
    public class AuthorizationResponseContext
    {
        public AuthorizationResponseContext(AuthorizationHandlerContext handlerContext)
        {
            HandlerContext = handlerContext;
        }

        public AuthorizationHandlerContext HandlerContext { get; }
    }
}
