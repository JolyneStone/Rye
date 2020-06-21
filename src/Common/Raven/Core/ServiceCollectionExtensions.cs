using Raven.DependencyInjection;
using Raven.Options;
using Raven.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using Raven.Configuration;

namespace Raven
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Raven框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRaven(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return AddRavenCore(serviceCollection, null);
        }

        /// <summary>
        /// 添加Raven框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddRaven(this IServiceCollection serviceCollection, Action<RavenOptions> action)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return AddRavenCore(serviceCollection, action);
        }

        private static IServiceCollection AddRavenCore(IServiceCollection serviceCollection, Action<RavenOptions> action)
        {
            serviceCollection.AddOptions();
            if (action != null)
            {
                serviceCollection.Configure<RavenOptions>(action);
            }
            else
            {
                serviceCollection.AddOptions<RavenOptions>();
            }
            serviceCollection.TryAddSingleton<IConfigureOptions<RavenOptions>, RavenOptionsSetup>();
            serviceCollection.TryAddSingleton<ISearcher<Assembly>, AssemblySeracher>();
            var services = serviceCollection.BuildServiceProvider();
            using(var scope = services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var section = configuration.GetSection("Raven:AssemblyPatterns");
                var patterns = section.Get<string[]>();
                //var options = scope.ServiceProvider.GetRequiredService<IOptions<RavenOptions>>().Value;
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
