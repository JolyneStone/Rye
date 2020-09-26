using Monica.AspectFlare.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Monica.AspectFlare;
using Monica.AspectFlare.DependencyInjection;

namespace Monica
{
    public static class AspectFlareServiceCollectionExtensions
    {
        public static IServiceCollection UseDynamicProxyService(this IServiceCollection services, bool isValid)
        {
            return UseDynamicProxyService(services, new ProxyFlare().UseDefaultProviders(isValid));
        }

        public static IServiceCollection UseDynamicProxyService(this IServiceCollection services)
        {
            return UseDynamicProxyService(services, new ProxyFlare().UseDefaultProviders());
        }

        public static IServiceCollection UseDynamicProxyService(this IServiceCollection services, IProxyFlare proxyFlare)
        {
            if (services == null)
            {
                throw new System.ArgumentNullException(nameof(services));
            }

            services
                .AddSingleton<IProxyFlare>(proxyFlare)
                .AddSingleton<IProxyConfiguration>(proxyFlare.GetConfiguration())
                .AddSingleton<IProxyCollection>(proxyFlare.GetCollection())
                .AddSingleton<IProxyProvider>(proxyFlare.GetProvider())
                .AddScoped<IProxyValidator>(_ => proxyFlare.GetValidator())
                .AddScoped<IProxyTypeGenerator>(_ => proxyFlare.GetTypeGenerator());

            var proxyServices = new ProxyServiceCollection(services, proxyFlare);
            proxyServices.AddSingleton(proxyServices);

            return proxyServices;
        }
    }
}
