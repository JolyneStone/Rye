using System;

namespace Monica.AspectFlare.DynamicProxy
{
    public static class ProxyFlareExtensions
    {
        public static IProxyFlare UseDefaultProviders(this IProxyFlare proxyFlare, bool isValid)
        {
            if (proxyFlare == null)
            {
                throw new ArgumentNullException(nameof(proxyFlare));
            }

            proxyFlare.Use(new ProxyConfigurationFactory())
                      .Use(new ProxyCollectionFactory())
                      .Use(new ProxyValidatorFactory())
                      .Use(new ProxyTypeGeneratorFactory())
                      .Use(new ProxyProviderFactory(isValid));

            return proxyFlare;
        }

        public static IProxyFlare UseDefaultProviders(this IProxyFlare proxyFlare)
        {
            return UseDefaultProviders(proxyFlare, true);
        }
    }
}
