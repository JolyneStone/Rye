using System;

namespace Monica.AspectFlare.DynamicProxy
{
    public interface IProxyCollection
    {
        void Add(Type interfaceType, Type classType, Type proxyType);
        Type GetProxyType(Type interfaceType, Type classType);
    }
}
