using System;

namespace Raven.AspectFlare.DynamicProxy
{
    public class ProxyProviderFactory : IProxyProviderFactory
    {
        private readonly bool _isValid;

        public ProxyProviderFactory(bool isValid)
        {
            _isValid = isValid;
        }
        public IProxyProvider BuilderProvider(IProxyConfiguration configuration)
        {
            return new ProxyProvider(configuration, _isValid);
        }
    }
}
