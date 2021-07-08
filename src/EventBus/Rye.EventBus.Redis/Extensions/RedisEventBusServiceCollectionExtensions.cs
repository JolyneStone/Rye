using Microsoft.Extensions.DependencyInjection;

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
            //var options = new RedisEventBusOptions();
            //action(options);

            serviceCollection.Configure(action);
            serviceCollection.AddEventBus<IRedisEventBus, RedisEventBus>();
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

        /// <summary>
        /// 订阅Redis EventBus
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection SubscriberRedisEventBus(this IServiceCollection serviceCollection, Action<IServiceProvider, IRedisEventBus> subscriberAction)
        {
            return serviceCollection.Subscriber<IRedisEventBus>(subscriberAction);
        }
    }
}
