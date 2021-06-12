using Microsoft.Extensions.Localization;

using System;

namespace Demo.Localization.StringLocalizer
{
    /// <summary>
    /// 用于创建SqlStringLocalizer的工厂类
    /// </summary>
    public class SqlStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly SqlStringLocalizerOptions _options;

        public SqlStringLocalizerFactory(SqlStringLocalizerOptions options)
        {
            _options = options;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new SqlStringLocalizer(_options);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new SqlStringLocalizer(_options);
        }
    }
}
