using Microsoft.AspNetCore.Mvc;

namespace Rye.Web.ResponseProvider.Security
{
    public interface ISecurityResponseProvider
    {
        string CreateParameterErrorResponse(SecurityContext context);
        string CreateEncryptErrorResponse(SecurityContext context);
        string CreateDecryptErrorResponse(SecurityContext context);
    }
}
