using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Application;
using Rye.EventBus.Application.Options;
using Rye.EventBus.Application.Policy;

using System;

namespace Rye.EventBus
{
    public static class ApplicationEventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 增加针对单体应用的事件总线模块的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">配置模块</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationEventBusModule(this IServiceCollection servicesCollection, Action<ApplicationEventBusOptions> action = null)
        {
            var module = new ApplicationEventBusModule(action);
            return servicesCollection.AddModule<ApplicationEventBusModule>(module);
        }

        /// <summary>
        /// 增加针对单体应用的事件总线模块的支持
        /// </summary>
        /// <param name="servicesCollection"></param>
        /// <param name="action">配置事件总线</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationEventBus(this IServiceCollection serviceCollection, Action<ApplicationEventBusOptions> action = null)
        {
            var options = new ApplicationEventBusOptions();
            //options.ProducerRetryPolicy = ApplicationEventBusRetryPolicy.ProducerRetryPolicy();
            //options.ConsumerRetryPolicy = ApplicationEventBusRetryPolicy.ConsumerRetryPolicy();
            action?.Invoke(options);
            serviceCollection.AddEventBus<IApplicationEventBus>(service => new ApplicationEventBus(options, service));
            serviceCollection.AddEventPublisher<IApplicationEventPublisher>(service => service.GetService<IApplicationEventBus>());
            serviceCollection.AddEventSubscriber<IApplicationEventSubscriber>(service => service.GetService<IApplicationEventBus>());
            serviceCollection.AddEventBus<IEventBus>(sevice => sevice.GetService<IApplicationEventBus>());
            serviceCollection.AddEventPublisher<IEventPublisher>(sevice => sevice.GetService<IApplicationEventBus>());
            serviceCollection.AddEventSubscriber<IEventSubscriber>(sevice => sevice.GetService<IApplicationEventBus>());

            return serviceCollection;
        }

        /// <summary>
        /// 订阅Application EventBus
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection SubscriberApplicationEventBus(this IServiceCollection serviceCollection, Action<IServiceProvider, IApplicationEventBus> subscriberAction)
        {
            return serviceCollection.Subscriber<IApplicationEventBus>(subscriberAction);
        }
    }
}
