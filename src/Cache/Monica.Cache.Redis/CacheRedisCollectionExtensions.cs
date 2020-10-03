using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.Cache;
using System;

namespace Monica
{
    public static class CacheRedisCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对Redis缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaCacheRedis(this IServiceCollection services, Action<RedisCacheOptions> action = null)
        {
            //将Redis分布式缓存服务添加到服务中
            if(action == null)
            {
                var congiration = services.GetSingletonInstance<IConfiguration>();
                action = options =>
                {
                    congiration.GetSection("Framework:RedisCache").Bind(options);
                };
            }
            services.AddDistributedRedisCache(action);
            services.TryAddSingleton<ICacheService, CacheService>();
            return services;
        }

        /// <summary>
        /// 添加Monica框架对Redis缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaCacheRedis(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var congiration = services.GetSingletonInstance<IConfiguration>();
            //将Redis分布式缓存服务添加到服务中
            services.AddDistributedRedisCache(options=> configurationSection.Bind(options));
            return services;
        }

    }
}
