using Rye.Web.ResponseProvider.Authorization;

namespace Rye.Web.Options
{
    public class AuthorizationOptions
    {
        public IAuthorizationResponseProvider Provider { get; set; } = new DefaultAuthorizationResponseProvider();
    }
}
