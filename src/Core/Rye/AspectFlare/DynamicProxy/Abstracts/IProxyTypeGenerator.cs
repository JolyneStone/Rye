using System;

namespace Rye.AspectFlare.DynamicProxy
{
    public interface IProxyTypeGenerator
    {
        Type GenerateProxyByClass(Type classType);
        Type GenerateProxyByInterface(Type interfaceType, Type classType);
    }
}
