using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Cache;
using Rye.Cache.Redis;

using System;

namespace Rye
{
    public static class CacheRedisCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeCacheRedis(this IServiceCollection serviceCollection, Action<RedisCacheOptions> action = null)
        {
            //将Redis分布式缓存服务添加到服务中
            if(action == null)
            {
                var congiration = serviceCollection.GetSingletonInstance<IConfiguration>();
                action = options =>
                {
                    congiration.GetSection("Framework:Redis").Bind(options);
                };
            }
            serviceCollection.AddDistributedRedisCache(action);
            serviceCollection.TryAddSingleton<ICacheService, CacheService>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configurationSection">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeCacheRedis(this IServiceCollection serviceCollection, IConfigurationSection configurationSection)
        {
            var congiration = serviceCollection.GetSingletonInstance<IConfiguration>();
            //将Redis分布式缓存服务添加到服务中
            serviceCollection.AddDistributedRedisCache(options=> configurationSection.Bind(options));
            return serviceCollection;
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheModule(this IServiceCollection serviceCollection, Action<RedisCacheOptions> action = null)
        {
            var module = new CacheRedisModule(action);
            return serviceCollection.AddModule<CacheRedisModule>(module);
        }
    }
}
