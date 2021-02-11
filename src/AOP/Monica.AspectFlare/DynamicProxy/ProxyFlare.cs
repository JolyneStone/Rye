//using System;

//namespace Monica.AspectFlare.DynamicProxy
//{
//    public class ProxyFlare : IProxyFlare
//    {
//        private IProxyProvider _provider;
//        private IProxyTypeGenerator _typeGenerator;
//        private IProxyValidator _validator;
//        private IProxyCollection _collection;
//        private IProxyConfiguration _configuration;

//        private static readonly Lazy<ProxyFlare> _flare = new Lazy<ProxyFlare>(() => new ProxyFlare(), true);

//        public static ProxyFlare Flare  => _flare.Value;

//        public IProxyFlare Use(IProxyProviderFactory providerFactory)
//        {
//            if (providerFactory == null)
//            {
//                throw new ArgumentNullException(nameof(providerFactory));
//            }

//            _provider = providerFactory.BuilderProvider(_configuration);
//            return this;
//        }

//        public IProxyFlare Use(IProxyValidatorFactory validatorFactory)
//        {
//            if (validatorFactory == null)
//            {
//                throw new ArgumentNullException(nameof(validatorFactory));
//            }

//            _validator = validatorFactory.BuilderValidator();
//            return this;
//        }

//        public IProxyFlare Use(IProxyTypeGeneratorFactory typeGeneratorFactory)
//        {
//            if (typeGeneratorFactory == null)
//            {
//                throw new ArgumentNullException(nameof(typeGeneratorFactory));
//            }

//            _typeGenerator = typeGeneratorFactory.BuilderTypeGenerator(_configuration);
//            return this;
//        }

//        public IProxyFlare Use(IProxyCollectionFactory collectionFactory)
//        {
//            if (collectionFactory == null)
//            {
//                throw new ArgumentNullException(nameof(collectionFactory));
//            }

//            _collection = collectionFactory.BuilderCollection();
//            return this;
//        }

//        public IProxyFlare Use(IProxyConfigurationFactory configurationFactory)
//        {
//            if (configurationFactory == null)
//            {
//                throw new ArgumentNullException(nameof(configurationFactory));
//            }

//            _configuration = configurationFactory.BuildConfiguration();
//            return this;
//        }


//        public IProxyCollection GetCollection()
//        {
//            return _collection ?? throw new InvalidOperationException("Cannot get the value of this object.");
//        }

//        public IProxyConfiguration GetConfiguration()
//        {
//            return _configuration ?? throw new InvalidOperationException("Cannot get the value of this object.");
//        }

//        public IProxyProvider GetProvider()
//        {
//            return _provider ?? throw new InvalidOperationException("Cannot get the value of this object.");
//        }

//        public IProxyTypeGenerator GetTypeGenerator()
//        {
//            return _typeGenerator ?? throw new InvalidOperationException("Cannot get the value of this object.");
//        }

//        public IProxyValidator GetValidator()
//        {
//            return _validator ?? throw new InvalidOperationException("Cannot get the value of this object.");
//        }
//    }
//}
