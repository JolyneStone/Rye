using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.Localization;

using System;

namespace Rye
{
    public static class LocalizationServiceCollectionExtensions
    {
        /// <summary>
        /// 配置多语言服务
        /// </summary>
        /// <typeparam name="TLangType">语言类型，表示资源所在的程序集中的任意一个类</typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLocalization<TLangType>(this IServiceCollection serviceCollection, Action<LocalizationSettingOptions> action = null)
            where TLangType : class
        {
            I18n.LangType = typeof(TLangType);
            if (action != null)
            {
                serviceCollection.Configure<LocalizationSettingOptions>(action);
            }
            else
            {
                serviceCollection.Configure<LocalizationSettingOptions>(App.Configuration.GetSection("Framework:LocalizationSetting"));
            }

            // 获取多语言配置选项
            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var localizationOptions = serviceProvider.GetService<IOptions<LocalizationSettingOptions>>().Value;
            // 如果没有配置多语言选项，则不注册服务
            if (localizationOptions.SupportedCultures == null || localizationOptions.SupportedCultures.Length == 0) return serviceCollection;

            // 注册多语言服务
            serviceCollection.AddLocalization(options =>
            {
                if (!string.IsNullOrWhiteSpace(localizationOptions.ResourcesPath))
                    options.ResourcesPath = localizationOptions.ResourcesPath;
            });

            //serviceCollection.RemoveAll<IStringLocalizerFactory>(); // 删除默认的服务，重新注入

            //serviceCollection.TryAddSingleton<SqlStringLocalizerFactory>(services => new SqlStringLocalizerFactory(options));
            //serviceCollection.TryAddSingleton<ResourceManagerStringLocalizerFactory>();

            //serviceCollection.TryAddSingleton<IStringLocalizerFactory, MutilStringLocalizerFactory>();

            return serviceCollection;
        }

    }
}
