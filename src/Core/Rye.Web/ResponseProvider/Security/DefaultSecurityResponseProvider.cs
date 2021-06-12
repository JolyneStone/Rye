
using Rye.Web.Internal;

using System.Net;

namespace Rye.Web.ResponseProvider.Security
{
    public class DefaultSecurityResponseProvider : ISecurityResponseProvider
    {
        public string CreateDecryptErrorResponse(SecurityContext context)
        {
            return Result.Create(HttpStatusCode.BadRequest, I18n.GetText(LangKeyEnum.DecryptError)).ToJsonString();
        }

        public string CreateEncryptErrorResponse(SecurityContext context)
        {
            return Result.Create(HttpStatusCode.BadRequest, I18n.GetText(LangKeyEnum.EncryptError)).ToJsonString();
        }

        public string CreateParameterErrorResponse(SecurityContext context)
        {
            return Result.Create(HttpStatusCode.BadRequest, I18n.GetText(LangKeyEnum.ParameterError)).ToJsonString();
        }
    }
}
