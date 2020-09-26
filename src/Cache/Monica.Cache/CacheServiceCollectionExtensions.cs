using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.Cache;
using Monica.Module;

namespace Monica
{
    public static class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对内存缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaCacheMemory(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.TryAddSingleton<ICacheService, CacheService>();
            return services;
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCacheModule(this IServiceCollection services)
        {
            return services.AddModule<CacheModule>();
        }
    }
}
