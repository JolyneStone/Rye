using CSRedis;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Cache;
using Rye.Cache.Redis;
using Rye.Cache.Redis.Internal;
using Rye.Cache.Redis.Options;

using System;

namespace Rye
{
    public static class CacheRedisCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="redisClient">Redis 客户端访问对象</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeCacheRedis(this IServiceCollection serviceCollection, CSRedisClient redisClient)
        {
            //将Redis分布式缓存服务添加到服务中
            //if(action == null)
            //{
            //    var congiration = serviceCollection.GetSingletonInstance<IConfiguration>();
            //    action = options =>
            //    {
            //        congiration.GetSection("Framework:Redis").Bind(options);
            //    };
            //}
            RedisHelper.Initialization(redisClient);
            serviceCollection.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            serviceCollection.TryAddSingleton<ICacheService, CacheService>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="func">Redis 客户端访问对象创建工厂</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeCacheRedis(this IServiceCollection serviceCollection, Func<CSRedisClient> func)
        {
            return AddRyeCacheRedis(serviceCollection, func());
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="options">Redis 配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeCacheRedis(this IServiceCollection serviceCollection, RedisOptions options)
        {
            return AddRyeCacheRedis(serviceCollection, new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options)));
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action">配置Redis 配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRyeCacheRedis(this IServiceCollection serviceCollection, Action<RedisOptions> action)
        {
            var options = new RedisOptions();
            action?.Invoke(options);
            return AddRyeCacheRedis(serviceCollection, new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options)));
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="redisClient">Redis 客户端访问对象</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheModule(this IServiceCollection serviceCollection, CSRedisClient redisClient)
        {
            var module = new CacheRedisModule(redisClient);
            return serviceCollection.AddModule<CacheRedisModule>(module);
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="func">Redis 客户端访问对象创建工厂</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheModule(this IServiceCollection serviceCollection, Func<CSRedisClient> func)
        {
            return AddRedisCacheModule(serviceCollection, func());
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="options">Redis 配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheModule(this IServiceCollection serviceCollection, RedisOptions options)
        {
            return AddRedisCacheModule(serviceCollection, new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options)));
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action">配置Redis 配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheModule(this IServiceCollection serviceCollection, Action<RedisOptions> action)
        {
            var options = new RedisOptions();
            action?.Invoke(options);
            return AddRedisCacheModule(serviceCollection, new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options)));
        }
    }
}
