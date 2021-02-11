using Monica.Web.ResponseProvider.Authorization;

namespace Monica.Web.Options
{
    public class AuthorizationOptions
    {
        public IAuthorizationResponseProvider Provider { get; set; } = new DefaultAuthorizationResponseProvider();
    }
}
