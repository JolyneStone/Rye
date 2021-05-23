using DotNetCore.CAP;

using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;

using System;

namespace Rye.EventBus.CAP
{
    public static class CapEventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 增加Cap分布式事件总线
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceCollection AddCapEventBus(this IServiceCollection serviceCollection, Action<CapOptions> action)
        {
            serviceCollection.AddCap(action);

            serviceCollection.AddEventBus<ICapEventBus>(services=> new CapEventBus(services.GetRequiredService<ICapPublisher>()));
            serviceCollection.AddEventPublisher<ICapEventPublisher>(service => service.GetService<ICapEventBus>());
            serviceCollection.AddEventSubscriber<ICapEventSubscriber>(service => service.GetService<ICapEventBus>());
            serviceCollection.AddEventBus<IEventBus>(sevice => sevice.GetService<ICapEventBus>());
            serviceCollection.AddEventPublisher<IEventPublisher>(sevice => sevice.GetService<ICapEventBus>());
            serviceCollection.AddEventSubscriber<IEventSubscriber>(sevice => sevice.GetService<ICapEventBus>());
            return serviceCollection;
        }

        /// <summary>
        /// 添加适用于Rabbit MQ的事件总线模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddCapEventBusModule(this IServiceCollection serviceCollection, Action<CapOptions> action)
        {
            var module = new CapEventBusModule(action);
            return serviceCollection.AddModule(module);
        }

        /// <summary>
        /// 订阅Rabbit MQ EventBus
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection SubscriberCapEventBus(this IServiceCollection serviceCollection, Action<IServiceProvider, ICapEventBus> subscriberAction)
        {
            return serviceCollection.Subscriber<ICapEventBus>(subscriberAction);
        }
    }
}
