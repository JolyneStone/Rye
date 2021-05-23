using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Rye.DependencyInjection;
using Rye.Logger;
using Rye.Options;
using Rye.Reflection;
using Rye.Security;

using System;
using System.Linq;
using System.Reflection;

namespace Rye
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRye(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return AddRyeCore(serviceCollection, null);
        }

        /// <summary>
        /// 添加Rye框架服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddRye(this IServiceCollection serviceCollection, Action<RyeOptions> action)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return AddRyeCore(serviceCollection, action);
        }

        private static IServiceCollection AddRyeCore(IServiceCollection serviceCollection, Action<RyeOptions> action)
        {
            serviceCollection.AddOptions();
            if (action == null)
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                action = options => serviceProvider.GetService<IConfiguration>().GetSection("Framework").Bind(options);
            }


            serviceCollection.Configure<RyeOptions>(action);
            //if (action != null)
            //{
            //    serviceCollection.Configure<RyeOptions>(action);
            //}
            //else
            //{
            //    serviceCollection.AddOptions<RyeOptions>();
            //}
            //serviceCollection.TryAddSingleton<IConfigureOptions<RyeOptions>, RyeOptionsSetup>();
            serviceCollection.TryAddSingleton<ISearcher<Assembly>, AssemblySeracher>();
            using (var services = serviceCollection.BuildServiceProvider())
            {
                using (var scope = services.CreateScope())
                {
                    var options = scope.ServiceProvider.GetRequiredService<IOptions<RyeOptions>>();
                    if (options.Value.Logger.UseDefaultLog)
                    {
                        serviceCollection.AddLogging(builder => builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, RyeLoggerProvider>()));
                        LogRecord.Options = options.Value?.Logger;
                    }
                    if (options.Value.AutoInjection)
                    {
                        serviceCollection.AutoInject();
                    }

                    return serviceCollection;
                }
            }
        }

        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AutoInject(this IServiceCollection serviceCollection)
        {
            using (var services = serviceCollection.BuildServiceProvider())
            {
                using (var scope = services.CreateScope())
                {
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var section = configuration.GetSection("Framework:AssemblyPatterns");
                    var patterns = section.Get<string[]>();
                    //var options = scope.ServiceProvider.GetRequiredService<IOptions<RyeOptions>>().Value;
                    LoadInjector.Load(serviceCollection, scope.ServiceProvider.GetRequiredService<ISearcher<Assembly>>(), patterns);
                    return serviceCollection;
                }
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        public static IConfiguration GetConfiguration(this IServiceCollection serviceCollection)
        {
            return serviceCollection.GetSingletonInstance<IConfiguration>();
        }

        /// <summary>
        /// 获取单例注册服务对象
        /// </summary>
        public static T GetSingletonInstance<T>(this IServiceCollection serviceCollection)
        {
            ServiceDescriptor descriptor = serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(T) && d.Lifetime == ServiceLifetime.Singleton);

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

        /// <summary>
        /// 添加安全加密支持
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddSecuritySupport<T>(this IServiceCollection serviceCollection)
            where T: class, ISecurityService
        {
            return serviceCollection.AddSingleton<ISecurityService, T>();
        }

        /// <summary>
        /// 添加安全加密支持
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IServiceCollection AddSecuritySupport<T>(this IServiceCollection serviceCollection, Func<IServiceProvider, T> factory)
            where T : class, ISecurityService
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return serviceCollection.AddSingleton<ISecurityService, T>(factory);
        }
    }
}
