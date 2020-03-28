using KiraNet.AlasFx.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KiraNet.AlasFx.DependencyInjection
{
    internal class LoadInjector
    {
        internal static void Load(IServiceCollection serviceCollection, ISearcher<Assembly> searcher, string[] patterns)
        {
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
            var assemblies = searcher.Get(assembly => !hasMatch ? true : patterns.Any(d => Regex.IsMatch(assembly.FullName, d)), true);
            if (assemblies == null)
                return;
            var typeDict = new Dictionary<Type, InjectionAttribute>();
            var classList = new List<Type>();
            var ignoreInjectionAttrType = typeof(IgnoreInjectionAttribute);
            var injectionAttrType = typeof(InjectionAttribute);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (!type.IsDefined(ignoreInjectionAttrType))
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
                }
            }

            foreach (var keyvalue in typeDict)
            {
                var type = keyvalue.Key;
                var attr = keyvalue.Value;
                if (type.IsInterface || type.IsAbstract)
                {
                    foreach (var classType in classList.Where(d => type.IsAssignableFrom(d) && !d.IsDefined(ignoreInjectionAttrType)).ToArray())
                    {
                        var classAttr = classType.GetCustomAttribute<InjectionAttribute>() ?? attr;
                        AddService(serviceCollection, type, classType, classAttr);
                    }
                }
                else
                {
                    AddService(serviceCollection, attr.ServiceType ?? type, type, attr);
                }
            }
        }

        private static void AddService(IServiceCollection serviceCollection, Type serviceType, Type implementationType, InjectionAttribute injectionAttribute)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, injectionAttribute.Lifetime);
            switch (injectionAttribute.Policy)
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
