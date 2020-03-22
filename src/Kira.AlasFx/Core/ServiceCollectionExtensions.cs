using Kira.AlasFx.DependencyInjection;
using Kira.AlasFx.Log;
using Kira.AlasFx.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using System;

namespace Kira.AlasFx
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFx(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.TryAddSingleton<IConfigureOptions<AlasFxOptions>, AlasFxOptionsSetup>();
            serviceCollection.AddLogging(builder => builder.AddAlasFxLog());

            SingleServiceLocator.SetServiceCollection(serviceCollection);
            return serviceCollection;
        }

        public static ILoggingBuilder AddAlasFxLog(this ILoggingBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AlasFxLoggerProvider>());
            return builder;
        }
    }
}
