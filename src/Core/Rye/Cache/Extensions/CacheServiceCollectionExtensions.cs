using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Cache;
using Rye.Cache.Store;

namespace Rye
{
    public static class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架对内存缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCacheMemory(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.TryAddSingleton<IMemoryStore, MemoryStore>();
            services.TryAddSingleton<ICacheStore>(services => services.GetRequiredService<IMemoryStore>());
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
