
using Rye.Web.Internal;

using System.Net;

namespace Rye.Web.ResponseProvider.Security
{
    public class DefaultSecurityResponseProvider : ISecurityResponseProvider
    {
        public string CreateDecryptErrorResponse(SecurityContext context)
        {
            return Result.Create(HttpStatusCode.BadRequest, $"Decrypt Error").ToJsonString();
        }

        public string CreateEncryptErrorResponse(SecurityContext context)
        {
            return Result.Create(HttpStatusCode.BadRequest, $"Encrypt Error").ToJsonString();
        }

        public string CreateParameterErrorResponse(SecurityContext context)
        {
            return Result.Create(HttpStatusCode.BadRequest, $"Parameter Error").ToJsonString();
        }
    }
}
