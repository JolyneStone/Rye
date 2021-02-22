using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Business;
using Rye.Business.Options;
using Rye.Business.QRCode;
using Rye.Business.Validate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddRyeBusiness(this IServiceCollection serviceCollection, Action<BusinessOptions> action = null)
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
            serviceCollection.TryAddSingleton<IQRCodeService, QRCodeService>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configurationSection">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeBusiness(this IServiceCollection serviceCollection, IConfigurationSection configurationSection)
        {
            var congiration = serviceCollection.GetSingletonInstance<IConfiguration>();
            serviceCollection.Configure<BusinessOptions>(options => configurationSection.Bind(options));
            serviceCollection.TryAddSingleton<IVerifyCodeService, VerifyCodeService>();
            serviceCollection.TryAddSingleton<IQRCodeService, QRCodeService>();
            return serviceCollection;
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
