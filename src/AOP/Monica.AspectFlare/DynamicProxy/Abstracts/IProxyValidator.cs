using System;

namespace Monica.AspectFlare
{
    public interface IProxyValidator
    {
        bool Validate(Type classType);
        bool Validate(Type interfaceType, Type classType);
    }
}
