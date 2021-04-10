using CSRedis;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Cache;
using Rye.Cache.Redis;
using Rye.Cache.Redis.Builder;
using Rye.Cache.Redis.Internal;
using Rye.Cache.Redis.Options;
using Rye.Cache.Redis.Store;
using Rye.Cache.Store;

using System;

namespace Rye.Cache.Redis
{
    public static class RedisCacheCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="redisClient">Redis 客户端访问对象</param>
        /// <returns></returns>
        private static IServiceCollection AddRedisCache(IServiceCollection serviceCollection, CSRedisClient redisClient)
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
            return serviceCollection;
        }

        /// <summary>
        /// 添加Rye框架对Redis缓存的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action">配置Redis 配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection serviceCollection, Action<RedisOptions> action)
        {
            var options = new RedisOptions();
            action?.Invoke(options);

            var redisClient = new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options));
            RedisHelper.Initialization(redisClient);
            serviceCollection.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            if (options.MultiCacheEnabled)
            {
                serviceCollection.TryAddSingleton<IRedisStore>(service => new RedisStore(options, service.GetRequiredService<IMemoryStore>()));
                serviceCollection.RemoveAll<ICacheStore>();
                serviceCollection.TryAddSingleton<ICacheStore>(service => service.GetRequiredService<IRedisStore>());
            }
            else
            {
                serviceCollection.TryAddSingleton<IRedisStore>(_ => new RedisStore(options));
            }
            return serviceCollection;
        }

        /// <summary>
        /// 添加缓存模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action">配置Redis 配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheModule(this IServiceCollection serviceCollection, Action<RedisOptions> action)
        {
            var redisModule = new RedisCacheModule(action);
            return serviceCollection.AddModule(redisModule);
        }
    }
}
