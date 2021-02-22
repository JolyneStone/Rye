using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rye.EventBus.Abstractions;
using Rye.EventBus.Memory;
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
        public static IServiceCollection AddMemoryEventBusModule(this IServiceCollection services)
        {
            return services.AddModule<MemoryEventBusModule>();
        }

        public static IServiceCollection AddMemoryEventBus(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IEventSubscriber, MemoryEventBus>();
            services.TryAddSingleton<IEventPublisher, MemoryEventBus>();
            services.TryAddSingleton<IEventBus, MemoryEventBus>();

            return services;
        }
    }
}
