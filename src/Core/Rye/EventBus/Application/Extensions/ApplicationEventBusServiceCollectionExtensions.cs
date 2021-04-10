using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Application;

using System;

namespace Rye
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
        public static IServiceCollection AddApplicationEventBus(this IServiceCollection servicesCollection, int bufferSize)
        {
            if (servicesCollection is null)
            {
                throw new ArgumentNullException(nameof(servicesCollection));
            }

            servicesCollection.TryAddSingleton<IApplicationEventBus>(_ => new ApplicationEventBus(bufferSize));
            servicesCollection.TryAddSingleton<IApplicationEventPublisher>(service => service.GetService<IApplicationEventBus>());
            servicesCollection.TryAddSingleton<IApplicationEventSubscriber>(service => service.GetService<IApplicationEventBus>());
            servicesCollection.TryAddSingleton<IEventBus>(service => service.GetService<IApplicationEventBus>());
            servicesCollection.TryAddSingleton<IEventPublisher>(service => service.GetService<IEventBus>());
            servicesCollection.TryAddSingleton<IEventSubscriber>(service => service.GetService<IEventBus>());

            return servicesCollection;
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
