using Rye.Web.ResponseProvider.Security;

namespace Rye.Web.Options
{
    public class SecurityOptions
    {
        public ISecurityResponseProvider Provider { get; set; } = new DefaultSecurityResponseProvider();
    }
}
