using Monica.Web.ResponseProvider.AvoidRepeatableRequestAttr;

namespace Monica.Web.Options
{
    public class AvoidRepeatableRequestOptions
    {
        public IAvoidRepeatableRequestResponseProvider Provider { get; set; } = new DefaultAvoidRepeatableRequestResponseProvider();
    }
}
