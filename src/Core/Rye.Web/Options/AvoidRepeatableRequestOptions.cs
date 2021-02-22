using Rye.Web.ResponseProvider.AvoidRepeatableRequestAttr;

namespace Rye.Web.Options
{
    public class AvoidRepeatableRequestOptions
    {
        public IAvoidRepeatableRequestResponseProvider Provider { get; set; } = new DefaultAvoidRepeatableRequestResponseProvider();
    }
}
