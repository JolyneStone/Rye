using System;

namespace Raven.AspectFlare.DynamicProxy
{
    public static class ProxyProviderExtensions
    {
        public static object GetProxy(this IProxyProvider proxyProvider, Type classType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            return proxyProvider.GetProxy(classType, (object[])null);
        }

        public static object GetProxy(this IProxyProvider proxyProvider, Type interfaceType, Type classType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            return proxyProvider.GetProxy(interfaceType, classType, (object[])null);
        }

        public static TClass GetProxy<TClass>(this IProxyProvider proxyProvider)
            where TClass : class
        {
            return proxyProvider.GetProxy(typeof(TClass), (object[])null) as TClass;
        }

        public static TClass GetProxy<TClass>(this IProxyProvider proxyProvider, params object[] parameters)
            where TClass : class
        {
            return proxyProvider.GetProxy(typeof(TClass), parameters) as TClass;
        }

        public static TInterface GetProxy<TInterface, TClass>(this IProxyProvider proxyProvider, params object[] parameters)
            where TInterface : class
            where TClass : TInterface
        {
            return proxyProvider.GetProxy(typeof(TInterface), typeof(TClass), parameters) as TInterface;
        }
    }
}
