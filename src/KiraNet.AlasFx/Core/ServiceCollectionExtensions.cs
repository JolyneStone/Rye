using KiraNet.AlasFx.DependencyInjection;
using KiraNet.AlasFx.Log;
using KiraNet.AlasFx.Options;
using KiraNet.AlasFx.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace KiraNet.AlasFx
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFx(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IConfigureOptions<AlasFxOptions>, AlasFxOptionsSetup>();
            return AddAlasFxCore(serviceCollection);
        }

        /// <summary>
        /// 添加AlasFx框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFx(this IServiceCollection serviceCollection, Action<AlasFxOptions> action)
        {
            serviceCollection.Configure<AlasFxOptions>(action);
            return AddAlasFxCore(serviceCollection);
        }

        private static IServiceCollection AddAlasFxCore(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddLogging(builder => builder.AddAlasFxLog());
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddSingleton<ISearcher<Assembly>, AssemblySeracher>();

            var services = serviceCollection.BuildServiceProvider();
            using(var scope = services.CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<AlasFxOptions>();
                LoadInjector.Load(serviceCollection, scope.ServiceProvider.GetRequiredService<ISearcher<Assembly>>(), options.AssemblyPatterns);
            }

            SingleServiceLocator.SetServiceCollection(serviceCollection);
            return serviceCollection;
        }

        private static ILoggingBuilder AddAlasFxLog(this ILoggingBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AlasFxLoggerProvider>());
            return builder;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IConfiguration>();
        }

        /// <summary>
        /// 获取单例注册服务对象
        /// </summary>
        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            ServiceDescriptor descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T) && d.Lifetime == ServiceLifetime.Singleton);

            if (descriptor?.ImplementationInstance != null)
            {
                return (T)descriptor.ImplementationInstance;
            }

            if (descriptor?.ImplementationFactory != null)
            {
                return (T)descriptor.ImplementationFactory.Invoke(null);
            }

            return default;
        }
    }
}
