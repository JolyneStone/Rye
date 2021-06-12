using System;

namespace Rye.Localization
{
    /// <summary>
    /// 多语言配置选项
    /// </summary>
    public sealed class LocalizationSettingOptions 
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResourcesPath { get; set; } = "Resources";

        /// <summary>
        /// 支持的语言列表
        /// </summary>
        public string[] SupportedCultures { get; set; }

        /// <summary>
        /// 默认的语言
        /// </summary>
        public string DefaultCulture { get; set; }
    }
}