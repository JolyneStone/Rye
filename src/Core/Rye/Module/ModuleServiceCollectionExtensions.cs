using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.DependencyInjection;
using Rye.Module;
using Rye.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rye
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

        /// <summary>
        /// 注入模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModule>(this IServiceCollection serviceCollection, TModule module)
        {
            Check.NotNull(module, nameof(module));
            AddModule(serviceCollection, (object)module);
            return serviceCollection;
        }

        private static void AddModule(IServiceCollection serviceCollection, Type moduleType, params object[] args)
        {
            if (moduleType == null)
                return;

            var set = new HashSet<Type>();
            AddModule(moduleType, set);
            var interfaceType = typeof(IStartupModule);
            var moduleInstance = Activator.CreateInstance(moduleType,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance,
                        null,
                        args,
                        null);
            foreach (var type in set)
            {
                if (type == moduleType)
                {
                    serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, moduleInstance));
                }
                else
                {
                    serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, type));
                }
            }
        }

        private static void AddModule(IServiceCollection serviceCollection, Type moduleType)
        {
            if (moduleType is null)
                return;

            var set = new HashSet<Type>();
            AddModule(moduleType, set);
            var interfaceType = typeof(IStartupModule);
            foreach (var type in set)
            {
                serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, type));
            }
        }

        private static void AddModule(IServiceCollection serviceCollection, object moduleInstance)
        {
            if (moduleInstance is null)
            {
                return;
            }

            var set = new HashSet<Type>();
            var moduleType = moduleInstance.GetType();
            AddModule(moduleType, set);
            var interfaceType = typeof(IStartupModule);
            foreach (var type in set)
            {
                if(type == moduleType)
                {
                    serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, moduleInstance));
                }
                else
                {
                    serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, type));
                }
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

            SingleServiceLocator.SetServiceCollection(serviceCollection);
            return serviceCollection;
        }

        private static void InjectModules(IServiceCollection serviceCollection)
        {
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scope = serviceProvider.CreateScope())
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
        }

        /// <summary>
        /// 注入Rye核心模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreModule(this IServiceCollection serviceCollection, Action<RyeOptions> action = null)
        {
            var module = new RyeCoreModule(action);
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupModule>(module));
            return serviceCollection;
        }
    }
}
