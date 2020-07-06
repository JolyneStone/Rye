namespace Raven.AspectFlare.DynamicProxy
{
    public interface IProxyTypeGeneratorFactory
    {
        IProxyTypeGenerator BuilderTypeGenerator(IProxyConfiguration configuration);
    }
}
