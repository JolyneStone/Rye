using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Lightweight;
using Rye.EventBus.Lightweight.Options;
using Rye.EventBus.Lightweight.Policy;

using System;

namespace Rye.EventBus
{
    public static class LightweightEventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 增加轻量级的事件总线模块的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">配置模块</param>
        /// <returns></returns>
        public static IServiceCollection AddLightweightEventBusModule(this IServiceCollection servicesCollection, Action<LightweightEventBusOptions> action = null)
        {
            var module = new LightweightEventBusModule(action);
            return servicesCollection.AddModule<LightweightEventBusModule>(module);
        }

        /// <summary>
        /// 增加轻量级的事件总线模块的支持
        /// </summary>
        /// <param name="servicesCollection"></param>
        /// <param name="action">配置事件总线</param>
        /// <returns></returns>
        public static IServiceCollection AddLightweightEventBus(this IServiceCollection serviceCollection, Action<LightweightEventBusOptions> action = null)
        {
            //var options = new LightweightEventBusOptions();
            //action?.Invoke(options);

            serviceCollection.Configure(action);
            serviceCollection.AddEventBus<ILightweightEventBus, LightweightEventBus>();
            serviceCollection.AddEventPublisher<ILightweightEventPublisher>(service => service.GetService<ILightweightEventBus>());
            serviceCollection.AddEventSubscriber<ILightweightEventSubscriber>(service => service.GetService<ILightweightEventBus>());
            serviceCollection.AddEventBus<IEventBus>(sevice => sevice.GetService<ILightweightEventBus>());
            serviceCollection.AddEventPublisher<IEventPublisher>(sevice => sevice.GetService<ILightweightEventBus>());
            serviceCollection.AddEventSubscriber<IEventSubscriber>(sevice => sevice.GetService<ILightweightEventBus>());

            return serviceCollection;
        }

        /// <summary>
        /// 订阅Application EventBus
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection SubscriberLightweightEventBus(this IServiceCollection serviceCollection, Action<IServiceProvider, ILightweightEventBus> subscriberAction)
        {
            return serviceCollection.Subscriber<ILightweightEventBus>(subscriberAction);
        }
    }
}
