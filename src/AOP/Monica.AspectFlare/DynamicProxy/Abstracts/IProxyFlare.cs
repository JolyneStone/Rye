namespace Monica.AspectFlare.DynamicProxy
{
    public interface IProxyFlare
    {
        IProxyFlare Use(IProxyProviderFactory providerFactory);
        IProxyFlare Use(IProxyValidatorFactory validatorFactory);
        IProxyFlare Use(IProxyTypeGeneratorFactory typeGeneratorFactory);
        IProxyFlare Use(IProxyCollectionFactory collectionFactory);
        IProxyFlare Use(IProxyConfigurationFactory configurationFactory);

        IProxyProvider GetProvider();
        IProxyValidator GetValidator();
        IProxyTypeGenerator GetTypeGenerator();
        IProxyCollection GetCollection();
        IProxyConfiguration GetConfiguration();
    }
}
