using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raven.EventBus.Abstractions;
using Raven.EventBus.Memory;
using System;

namespace Raven
{
    public static class ServiceCollectionExtensions
    {
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
