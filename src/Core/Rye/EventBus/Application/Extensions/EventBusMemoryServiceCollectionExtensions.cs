using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Application;

using System;

namespace Rye
{
    public static class EventBusMemoryServiceCollectionExtensions
    {
        /// <summary>
        /// 增加对内存间事件总线模块的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryEventBusModule(this IServiceCollection servicesCollection)
        {
            return servicesCollection.AddModule<ApplicationEventBusModule>();
        }

        public static IServiceCollection AddMemoryEventBus(this IServiceCollection servicesCollection)
        {
            if (servicesCollection is null)
            {
                throw new ArgumentNullException(nameof(servicesCollection));
            }

            servicesCollection.TryAddSingleton<IEventBus, ApplicationEventBus>();
            servicesCollection.TryAddSingleton<IEventSubscriber>(service => service.GetRequiredService<IEventBus>());
            servicesCollection.TryAddSingleton<IEventPublisher>(service => service.GetRequiredService<IEventBus>());

            return servicesCollection;
        }
    }
}
