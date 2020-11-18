using Monica.Web.ResponseProvider;

namespace Monica.Web.Options
{
    public class GlobalExceptionFilterOptions
    {
        /// <summary>
        /// 是否启用全局异常过滤器
        /// </summary>
        public bool Enabled { get; set; } = true;
        public IGlobalExceptionFilterResponseProvider Provider { get; set; } = new DefaultGlobalExceptionFilterResponseProvider();
    }
}
