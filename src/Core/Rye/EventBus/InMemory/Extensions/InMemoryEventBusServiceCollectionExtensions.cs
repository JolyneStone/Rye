using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;
using Rye.EventBus.InMemory;
using Rye.EventBus.InMemory.Options;
using Rye.EventBus.InMemory.Policy;

using System;

namespace Rye.EventBus
{
    public static class InMemoryEventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 增加针对内存的事件总线模块的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">配置模块</param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryEventBusModule(this IServiceCollection servicesCollection, Action<InMemoryEventBusOptions> action = null)
        {
            var module = new InMemoryEventBusModule(action);
            return servicesCollection.AddModule<InMemoryEventBusModule>(module);
        }

        /// <summary>
        /// 增加针对内存的事件总线模块的支持
        /// </summary>
        /// <param name="servicesCollection"></param>
        /// <param name="action">配置事件总线</param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection serviceCollection, Action<InMemoryEventBusOptions> action = null)
        {
            var options = new InMemoryEventBusOptions();
            //options.ProducerRetryPolicy = InMemoryEventBusRetryPolicy.ProducerRetryPolicy();
            //options.ConsumerRetryPolicy = InMemoryEventBusRetryPolicy.ConsumerRetryPolicy();
            action?.Invoke(options);
            serviceCollection.AddEventBus<IMemoryEventBus>(service => new InMemoryEventBus(options, service.GetRequiredService<IServiceScopeFactory>()));
            serviceCollection.AddEventPublisher<IMemoryEventPublisher>(service => service.GetService<IMemoryEventBus>());
            serviceCollection.AddEventSubscriber<IMemoryEventSubscriber>(service => service.GetService<IMemoryEventBus>());
            serviceCollection.AddEventBus<IEventBus>(sevice => sevice.GetService<IMemoryEventBus>());
            serviceCollection.AddEventPublisher<IEventPublisher>(sevice => sevice.GetService<IMemoryEventBus>());
            serviceCollection.AddEventSubscriber<IEventSubscriber>(sevice => sevice.GetService<IMemoryEventBus>());

            return serviceCollection;
        }

        /// <summary>
        /// 订阅Application EventBus
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection SubscriberInMemoryEventBus(this IServiceCollection serviceCollection, Action<IServiceProvider, IMemoryEventBus> subscriberAction)
        {
            return serviceCollection.Subscriber<IMemoryEventBus>(subscriberAction);
        }
    }
}
