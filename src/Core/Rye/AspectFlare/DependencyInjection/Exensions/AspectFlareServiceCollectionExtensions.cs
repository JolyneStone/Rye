using Rye.AspectFlare.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Rye.AspectFlare;
using Rye.AspectFlare.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Rye
{
    public static class AspectFlareServiceCollectionExtensions
    {
        ///// <summary>
        ///// 添加AOP模块
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddAopModule(this IServiceCollection services)
        //{
        //    return services.AddModule<AspectFlareModule>();
        //}

        /// <summary>
        /// 使用动态代理Service Collection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseDynamicProxyService(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new System.ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IProxyConfiguration, ProxyConfiguration>();
            services.TryAddSingleton<IProxyCollection, ProxyCollection>();
            services.TryAddSingleton<IProxyValidator, ProxyValidator>();
            services.TryAddSingleton<IProxyTypeGenerator, ProxyTypeGenerator>();
            services.TryAddSingleton<IProxyProvider, ProxyProvider>();

            IProxyProvider proxyProvider;
            using (var scope = services.BuildServiceProvider())
            {
                proxyProvider = scope.GetRequiredService<IProxyProvider>();
            }

            var proxyServices = new ProxyServiceCollection(services, proxyProvider);
            proxyServices.AddSingleton(proxyServices);

            return proxyServices;
        }
    }
}
