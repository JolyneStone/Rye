using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Business;
using Rye.Business.Options;
using Rye.Business.Validate;

using System;

namespace Rye
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection serviceCollection, Action<BusinessOptions> action = null)
        {
            if (action == null)
            {
                var congiration = serviceCollection.GetSingletonInstance<IConfiguration>();
                action = options =>
                {
                    congiration.GetSection("Framework:Business").Bind(options);
                };
            }

            serviceCollection.Configure<BusinessOptions>(action);
            serviceCollection.TryAddSingleton<IVerifyCodeService, VerifyCodeService>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configurationSection">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddBusiness(this IServiceCollection serviceCollection, IConfigurationSection configurationSection)
        {
            return AddBusiness(serviceCollection, options => configurationSection.Bind(options));
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessModule(this IServiceCollection services, Action<BusinessOptions> action = null)
        {
            var module = new RyeBusinessModule(action);
            return services.AddModule<RyeBusinessModule>(module);
        }
    }
}
