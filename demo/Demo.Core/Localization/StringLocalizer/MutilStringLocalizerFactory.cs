using Microsoft.Extensions.Localization;

using System;

namespace Demo.Localization.StringLocalizer
{
    /// <summary>
    /// MutilStringLocalizer的创建工厂
    /// </summary>
    public class MutilStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IStringLocalizerFactory[] _stringLocalizerFactories;

        public MutilStringLocalizerFactory(
            SqlStringLocalizerFactory sqlStringLocalizerFactory,
            ResourceManagerStringLocalizerFactory resourceManagerStringLocalizerFactory)
        {
            _stringLocalizerFactories = new IStringLocalizerFactory[]
            {
                sqlStringLocalizerFactory,
                resourceManagerStringLocalizerFactory,
            };
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            IStringLocalizer[] localizers = new IStringLocalizer[_stringLocalizerFactories.Length];
            for(var i = 0; i < _stringLocalizerFactories.Length; i++)
            {
                localizers[i] = _stringLocalizerFactories[i].Create(resourceSource);
            }
            return new MutilStringLocalizer(localizers);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            IStringLocalizer[] localizers = new IStringLocalizer[_stringLocalizerFactories.Length];
            for (var i = 0; i < _stringLocalizerFactories.Length; i++)
            {
                localizers[i] = _stringLocalizerFactories[i].Create(baseName, location);
            }
            return new MutilStringLocalizer(localizers);
        }
    }
}
