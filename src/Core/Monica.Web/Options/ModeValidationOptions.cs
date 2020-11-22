using Monica.Web.ResponseProvider.ModeValidationAttr;

namespace Monica.Web.Options
{
    public class ModeValidationOptions
    {
        public IModeValidationResponseProvider Provider { get; set; } = new DefaultModeValidationResponseProvider();
    }
}