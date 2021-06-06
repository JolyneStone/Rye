using Rye.Web.ResponseProvider;

namespace Rye.Web.Options
{
    public class GlobalActionFilterOptions
    {
        /// <summary>
        /// 是否启用全局过滤器
        /// </summary>
        public bool Enabled { get; set; } = true;

        public IGlobalActionFilterResponseProvider Provider { get; set; } = new DefaultGlobalActionFilterResponseProvider();
    }
}