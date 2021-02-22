using System;

namespace Rye.AspectFlare.DynamicProxy
{
    public interface IProxyProvider
    {
        object GetProxy(Type classType, params object[] parameters);
        object GetProxy(Type interfaceType, Type classType, params object[] parameters);
        bool TryGetProxyType(Type classType, out Type proxyType);
        bool TryGetProxyType(Type interfaceType, Type classType, out Type proxyType);
        Type GetProxyType(Type classType);
        Type GetProxyType(Type interfaceType, Type classType);
    }
}
