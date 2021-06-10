using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rye.DependencyInjection
{
    internal class LoadInjector
    {
        internal static void Load(IServiceCollection serviceCollection)
        {
            var typeDict = new Dictionary<Type, InjectionAttribute>();
            var classList = new List<Type>();
            var injectionAttrType = typeof(InjectionAttribute);
            foreach (var type in App.ScanTypes)
            {
                var attr = type.GetCustomAttribute<InjectionAttribute>(true);
                if (attr != null)
                {
                    typeDict.Add(type, attr);
                }
                if (!type.IsInterface && !type.IsAbstract && !type.IsEnum && !type.IsPrimitive)
                {
                    classList.Add(type);
                }
            }

            foreach (var keyvalue in typeDict)
            {
                var type = keyvalue.Key;
                var attr = keyvalue.Value;
                if (type.IsInterface || type.IsAbstract)
                {
                    foreach (var classType in classList.Where(d => type.IsAssignableFrom(d)).ToArray())
                    {
                        var classAttr = classType.GetCustomAttribute<InjectionAttribute>() ?? attr;
                        AddServices(serviceCollection,
                            type,
                            classType,
                            classAttr);
                    }
                }
                else
                {
                    AddServices(serviceCollection, type, type, attr);
                }
            }
        }

        private static void AddServices(IServiceCollection serviceCollection, Type serviceType, Type implementationType, InjectionAttribute injectionAttribute)
        {
            if (injectionAttribute.ServiceTypes == null || injectionAttribute.ServiceTypes.Length <= 0)
            {
                AddService(serviceCollection, serviceType, implementationType, injectionAttribute.Lifetime, injectionAttribute.Policy);
            }
            else
            {
                foreach (var type in injectionAttribute.ServiceTypes)
                {
                    AddService(serviceCollection, type, implementationType, injectionAttribute.Lifetime, injectionAttribute.Policy);
                }
            }
        }

        private static void AddService(IServiceCollection serviceCollection, Type serviceType, Type implementationType, ServiceLifetime lifetime, InjectionPolicy policy)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            switch (policy)
            {
                case InjectionPolicy.Append:
                    serviceCollection.TryAddEnumerable(descriptor);
                    break;
                case InjectionPolicy.Skip:
                    serviceCollection.TryAdd(descriptor);
                    break;
                case InjectionPolicy.Replace:
                    serviceCollection.Replace(descriptor);
                    break;
            }
        }
    }
}
