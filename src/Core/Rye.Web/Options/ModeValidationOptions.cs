using Rye.Web.ResponseProvider.ModeValidationAttr;

namespace Rye.Web.Options
{
    public class ModeValidationOptions
    {
        public IModeValidationResponseProvider Provider { get; set; } = new DefaultModeValidationResponseProvider();
    }
}