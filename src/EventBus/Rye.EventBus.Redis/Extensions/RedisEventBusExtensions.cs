using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Redis.Options;

using System;

namespace Rye.EventBus.Redis.Extensions
{
    public static class RedisEventBusExtensions
    {
        /// <summary>
        /// 添加适用于Redis的事件总线
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisEventBus(this IServiceCollection serviceCollection, Action<RedisEventBusOptions> action)
        {
            var options = new RedisEventBusOptions();
            action(options);

            serviceCollection.TryAddSingleton<IRedisEventBus>(_ => new RedisEventBus(options));
            serviceCollection.TryAddSingleton<IRedisEventPublisher>(service => service.GetService<IRedisEventBus>());
            serviceCollection.TryAddSingleton<IRedisEventSubscriber>(service => service.GetService<IRedisEventBus>());
            serviceCollection.RemoveAll<IEventBus>();
            serviceCollection.TryAddSingleton<IEventBus>(sevice => sevice.GetService<IRedisEventBus>());

            return serviceCollection;
        }

        /// <summary>
        /// 添加适用于Redis的事件总线模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisEventBusModule(this IServiceCollection serviceCollection, Action<RedisEventBusOptions> action)
        {
            var module = new RedisEventBusModule(action);
            return serviceCollection.AddModule(module);
        }
    }
}
