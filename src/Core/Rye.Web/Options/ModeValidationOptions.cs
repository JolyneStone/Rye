using Rye.Web.ResponseProvider.ModeValidationAttr;

namespace Rye.Web.Options
{
    public class ModeValidationOptions
    {
        /// <summary>
        /// 是否启用全局过滤器
        /// </summary>
        public bool Enabled { get; set; } = true;
        public IModeValidationResponseProvider Provider { get; set; } = new DefaultModeValidationResponseProvider();
    }
}