using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.AspectFlare.DynamicProxy;
using Rye.EventBus.Abstractions;
using Rye.EventBus.Redis;
using Rye.EventBus.Redis.Options;

using System;

namespace Rye.EventBus
{
    public static class RedisEventBusServiceCollectionExtensions
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

            serviceCollection.AddEventBus<IRedisEventBus>(service => new RedisEventBus(options, service));
            serviceCollection.AddEventPublisher<IRedisEventPublisher>(service => service.GetService<IRedisEventBus>());
            serviceCollection.AddEventSubscriber<IRedisEventSubscriber>(service => service.GetService<IRedisEventBus>());
            serviceCollection.AddEventBus<IEventBus>(sevice => sevice.GetService<IRedisEventBus>());
            serviceCollection.AddEventPublisher<IEventPublisher>(sevice => sevice.GetService<IRedisEventBus>());
            serviceCollection.AddEventSubscriber<IEventSubscriber>(sevice => sevice.GetService<IRedisEventBus>());

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
