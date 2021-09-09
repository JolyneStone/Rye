using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Rye.DependencyInjection;
using Rye.Logger;
using Rye.Options;
using Rye;
using Rye.Security;

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

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
            serviceCollection.RemoveAll<IConfiguration>();
            serviceCollection.RemoveAll<IConfigurationRoot>();
            serviceCollection.AddSingleton<IConfiguration>(App.Configuration);
            serviceCollection.AddSingleton<IConfigurationRoot>((IConfigurationRoot)App.Configuration);

            serviceCollection.AddOptions();
            if (action == null)
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                action = options => serviceProvider.GetService<IConfiguration>().GetSection("Framework").Bind(options);
            }

            serviceCollection.Configure<RyeOptions>(action);
            serviceCollection.AddTransient(
                  typeof(Lazy<>),
                  typeof(LazilyResolved<>));
            using (var services = serviceCollection.BuildServiceProvider())
            {
                using (var scope = services.CreateScope())
                {
                    var options = scope.ServiceProvider.GetRequiredService<IOptions<RyeOptions>>();
                    if (options.Value.Logger.UseDefaultLog)
                    {
                        serviceCollection.AddLogging(builder => builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, RyeLoggerProvider>()));
                        LogRecord.Options = options.Value?.Logger;
                        Log.Current = new DefaultStaticLog();
                    }

                    App.Assemblies = GetDenpencyAssemblies(scope.ServiceProvider);
                    App.ScanTypes = GetEffectiveTypes(App.Assemblies);
                    serviceCollection.AutoInject();
                    return serviceCollection;
                }
            }
        }

        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        private static IServiceCollection AutoInject(this IServiceCollection serviceCollection)
        {
            LoadInjector.Load(serviceCollection);
            return serviceCollection;
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
            where T : class, ISecurityService
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


        private static Assembly[] GetDenpencyAssemblies(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection("Framework:AssemblyPatterns");
            var patterns = section.Get<string[]>();
            var hasMatch = patterns != null && patterns.Length > 0;
            if (hasMatch)
            {
                for (var i = 0; i < patterns.Length; i++)
                {
                    patterns[i] = "^" + Regex.Escape(patterns[i])
                           .Replace("*", ".*")
                           .Replace("?", ".") + "$";
                }
            }

            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Rye 发布的包，或配置特定的包前缀
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(lib =>
                       !lib.Serviceable &&
                       ((lib.Type == "project" && !lib.Name.StartsWith("Microsoft")) || (lib.Type == "package" && lib.Name.StartsWith("Rye"))))
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)));

            return scanAssemblies.ToArray();
        }

        private static Type[] GetEffectiveTypes(Assembly[] assemblies)
        {
            return assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsDefineWithBaseAttribute(typeof(ScanAttribute)))).ToArray();
        }
    }
}
