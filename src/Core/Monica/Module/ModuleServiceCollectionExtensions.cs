using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.Module;
using Monica.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monica
{
    public static class ModuleServiceCollectionExtensions
    {
        /// <summary>
        /// 注入模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModule>(this IServiceCollection serviceCollection)
            where TModule : class, IStartupModule
        {
            AddModule(serviceCollection, typeof(TModule));
            return serviceCollection;
        }

        /// <summary>
        /// 注入模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModule, TOptions>(this IServiceCollection serviceCollection, Action<TOptions> action)
            where TModule : class, IStartupModule
        {
            AddModule(serviceCollection, typeof(TModule), action);
            return serviceCollection;
        }

        private static void AddModule(IServiceCollection serviceCollection, Type moduleType, params object[] args)
        {
            if (moduleType == null)
                return;

            var set = new HashSet<Type>();
            AddModule(moduleType, set);
            var interfaceType = typeof(IStartupModule);
            foreach (var type in set)
            {
                if (type == moduleType)
                {
                    serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType,
                        Activator.CreateInstance(type,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance,
                        null,
                        args,
                        null)));
                }
                else
                {
                    serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, type));
                }
            }
        }

        private static void AddModule(IServiceCollection serviceCollection, Type moduleType)
        {
            if (moduleType == null)
                return;

            var set = new HashSet<Type>();
            AddModule(moduleType, set);
            var interfaceType = typeof(IStartupModule);
            foreach (var type in set)
            {
                serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, type));
            }
        }

        private static void AddModule(Type type, HashSet<Type> set)
        {
            if (set.Contains(type))
            {
                return;
            }

            set.Add(type);
            var dependModules = type.GetCustomAttributes<DependsOnModulesAttribute>(true);
            if (dependModules == null || !dependModules.Any())
                return;

            var interfaceType = typeof(IStartupModule);
            foreach (var item in dependModules.SelectMany(d => d.DependedModuleTypes)
                .Where(d => d != null && interfaceType.IsAssignableFrom(d) && d.IsClass && !d.IsAbstract))
            {
                AddModule(item, set);
            }
        }

        public static IServiceCollection ConfigureModule(this IServiceCollection serviceCollection)
        {
            AddCoreModule(serviceCollection);
            InjectModules(serviceCollection);
            return serviceCollection;
        }

        private static void InjectModules(IServiceCollection serviceCollection)
        {
            using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var modules = scope.ServiceProvider.GetServices<IStartupModule>();
                if (modules == null || !modules.Any())
                    return;

                foreach (var module in modules.OrderBy(d => d, new ModuleComparer()))
                {
                    module.ConfigueServices(serviceCollection);
                }
            }
        }

        /// <summary>
        /// 启用模块配置
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseModule(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return null;
            var modules = serviceProvider.GetServices<IStartupModule>();
            if (modules == null || !modules.Any())
                return serviceProvider;

            foreach (var module in modules.OrderBy(d => d, new ModuleComparer()))
            {
                module.Configure(serviceProvider);
            }

            return serviceProvider;
        }

        /// <summary>
        /// 注入Monica核心模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreModule(this IServiceCollection serviceCollection, Action<MonicaOptions> action = null)
        {
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupModule>(_ => new MonicaCoreModule(action)));
            return serviceCollection;
        }
    }
}
