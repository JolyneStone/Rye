using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Application;

using System;

namespace Rye.EventBus
{
    public static class ApplicationEventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 增加针对单体应用的事件总线模块的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="bufferSize">设置缓存区大小</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationEventBusModule(this IServiceCollection servicesCollection, int bufferSize)
        {
            var module = new ApplicationEventBusModule(bufferSize);
            return servicesCollection.AddModule<ApplicationEventBusModule>(module);
        }

        /// <summary>
        /// 增加针对单体应用的事件总线模块的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationEventBusModule(this IServiceCollection servicesCollection)
        {
            return AddApplicationEventBusModule(servicesCollection, 1024);
        }

        /// <summary>
        /// 增加针对单体应用的事件总线模块的支持
        /// </summary>
        /// <param name="servicesCollection"></param>
        /// <param name="bufferSize">设置缓存区大小</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationEventBus(this IServiceCollection serviceCollection, int bufferSize)
        {
            serviceCollection.AddEventBus<IApplicationEventBus>(service => new ApplicationEventBus(bufferSize, service));
            serviceCollection.AddEventPublisher<IApplicationEventPublisher>(service => service.GetService<IApplicationEventBus>());
            serviceCollection.AddEventSubscriber<IApplicationEventSubscriber>(service => service.GetService<IApplicationEventBus>());
            serviceCollection.AddEventBus<IEventBus>(sevice => sevice.GetService<IApplicationEventBus>());
            serviceCollection.AddEventPublisher<IEventPublisher>(sevice => sevice.GetService<IApplicationEventBus>());
            serviceCollection.AddEventSubscriber<IEventSubscriber>(sevice => sevice.GetService<IApplicationEventBus>());

            return serviceCollection;
        }

        /// <summary>
        /// 增加针对单体应用的事件总线模块的支持
        /// </summary>
        /// <param name="servicesCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationEventBus(this IServiceCollection servicesCollection)
        {
            return AddApplicationEventBus(servicesCollection, 1024);
        }
    }
}
