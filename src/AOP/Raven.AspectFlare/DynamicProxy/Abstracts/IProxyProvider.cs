using System;

namespace Raven.AspectFlare.DynamicProxy
{
    public interface IProxyProvider
    {
        object GetProxy(Type classType, params object[] parameters);
        object GetProxy(Type interfaceType, Type classType, params object[] parameters);
        Type GetProxyType(Type classType);
        Type GetProxyType(Type interfaceType, Type classType);
    }
}
