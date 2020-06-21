using System;

namespace Raven.AspectFlare.DynamicProxy
{
    public sealed class ProxyDescriptor
    {
        public Type InterfaceType { get; set; }
        public Type ClassType { get; set; }
        public Type ProxyType { get; internal set; }
    }
}
