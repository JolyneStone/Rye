using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KiraNet.AlasFx.Redis
{
    public static class RedisCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架对Redis缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFxRedis(this IServiceCollection services)
        {
            var congiration = services.GetSingletonInstance<IConfiguration>();
            //将Redis分布式缓存服务添加到服务中
            services.AddDistributedRedisCache(options =>
            {
                congiration.GetSection("AlasFx:Redis").Bind(options);
            });
            return services;
        }

        /// <summary>
        /// 添加AlasFx框架对Redis缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFxRedis(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            var congiration = services.GetSingletonInstance<IConfiguration>();
            //将Redis分布式缓存服务添加到服务中
            services.AddDistributedRedisCache(setupAction);
            return services;
        }

        /// <summary>
        /// 添加AlasFx框架对Redis缓存的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection">配置Redis</param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFxRedis(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var congiration = services.GetSingletonInstance<IConfiguration>();
            //将Redis分布式缓存服务添加到服务中
            services.AddDistributedRedisCache(options=> configurationSection.Bind(options));
            return services;
        }

    }
}
