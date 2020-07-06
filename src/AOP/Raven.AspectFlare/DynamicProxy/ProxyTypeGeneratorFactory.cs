namespace Raven.AspectFlare.DynamicProxy
{
    public class ProxyTypeGeneratorFactory : IProxyTypeGeneratorFactory
    {
        public IProxyTypeGenerator BuilderTypeGenerator(IProxyConfiguration configuration)
        {
            return new ProxyTypeGenerator(configuration);
        }

        public IProxyTypeGenerator BuilderTypeGenerator()
        {
            return new ProxyTypeGenerator(new ProxyConfigurationFactory().BuildConfiguration());
        }
    }
}
