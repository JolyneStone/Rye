using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monica.DependencyInjection;
using Monica.Logger;
using Monica.Options;
using Monica.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace Monica
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonica(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return AddMonicaCore(serviceCollection, null);
        }

        /// <summary>
        /// 添加Monica框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonica(this IServiceCollection serviceCollection, Action<MonicaOptions> action)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return AddMonicaCore(serviceCollection, action);
        }

        private static IServiceCollection AddMonicaCore(IServiceCollection serviceCollection, Action<MonicaOptions> action)
        {
            serviceCollection.AddOptions();
            if (action == null)
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                action = options => serviceProvider.GetService<IConfiguration>().GetSection("Framework").Bind(options);
            }

            serviceCollection.Configure<MonicaOptions>(action);
            //if (action != null)
            //{
            //    serviceCollection.Configure<MonicaOptions>(action);
            //}
            //else
            //{
            //    serviceCollection.AddOptions<MonicaOptions>();
            //}
            serviceCollection.TryAddSingleton<IConfigureOptions<MonicaOptions>, MonicaOptionsSetup>();
            serviceCollection.TryAddSingleton<ISearcher<Assembly>, AssemblySeracher>();
            var services = serviceCollection.BuildServiceProvider();
            using (var scope = services.CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<IOptions<MonicaOptions>>();
                if (options.Value.Logger.UseMonicaLog)
                {
                    serviceCollection.AddLogging(builder => builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MonicaLoggerProvider>()));
                }
                LogRecord.Options = options.Value?.Logger;
            }
            return serviceCollection;
        }

        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AutoInject(this IServiceCollection serviceCollection)
        {
            var services = serviceCollection.BuildServiceProvider();
            using (var scope = services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var section = configuration.GetSection("Framework:AssemblyPatterns");
                var patterns = section.Get<string[]>();
                //var options = scope.ServiceProvider.GetRequiredService<IOptions<MonicaOptions>>().Value;
                LoadInjector.Load(serviceCollection, scope.ServiceProvider.GetRequiredService<ISearcher<Assembly>>(), patterns);
            }
            return serviceCollection;
        }

        /// <summary>
        /// 添加SingleServiceLocator支持，建议将此方法放在最后调用
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddSignleServiceLocator(this IServiceCollection serviceCollection)
        {
            SingleServiceLocator.SetServiceCollection(serviceCollection);
            return serviceCollection;
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
