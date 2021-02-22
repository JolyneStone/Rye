using System;

namespace Rye.AspectFlare
{
    public interface IProxyValidator
    {
        bool Validate(Type classType);
        bool Validate(Type interfaceType, Type classType);
    }
}
