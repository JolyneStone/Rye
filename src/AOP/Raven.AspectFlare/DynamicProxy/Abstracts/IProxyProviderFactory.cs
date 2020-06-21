namespace Raven.AspectFlare.DynamicProxy
{
    public interface IProxyProviderFactory
    {
        IProxyProvider BuilderProvider(IProxyConfiguration configuration);
    }
}
