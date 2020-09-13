using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.Cache;

namespace Monica
{
    public static class CacheCollectionExtensions
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
    }
}
