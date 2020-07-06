using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raven.Cache;

namespace Raven
{
    public static class CacheCollectionExtensions
    {
        /// <summary>
        /// 添加Raven框架对内存缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRavenCacheMemory(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.TryAddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}
