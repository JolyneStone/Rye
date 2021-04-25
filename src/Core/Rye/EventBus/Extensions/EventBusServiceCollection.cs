using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EventBus.Abstractions;

using System;
using System.Linq;

namespace Rye.EventBus
{
    public static class EventBusServiceCollection
    {
        /// <summary>
        /// 添加事件总线依赖
        /// </summary>
        /// <typeparam name="TEventBus"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="eventBus"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus<TEventBus>(this IServiceCollection serviceCollection, TEventBus eventBus)
           where TEventBus : class, IEventBus
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(eventBus, nameof(eventBus));

            serviceCollection.RemoveAll<TEventBus>();
            serviceCollection.TryAddSingleton<TEventBus>(eventBus);
            return serviceCollection;
        }

        /// <summary>
        /// 添加事件总线依赖
        /// </summary>
        /// <typeparam name="TEventBus"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="eventBusFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus<TEventBus>(this IServiceCollection serviceCollection, Func<IServiceProvider, TEventBus> eventBusFactory)
            where TEventBus: class, IEventBus
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(eventBusFactory, nameof(eventBusFactory));

            serviceCollection.RemoveAll<TEventBus>();
            serviceCollection.TryAddSingleton<TEventBus>(eventBusFactory);
            return serviceCollection;
        }

        /// <summary>
        /// 添加事件发布者依赖
        /// </summary>
        /// <typeparam name="TEventPublisher"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="eventPublisher"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventPublisher<TEventPublisher>(this IServiceCollection serviceCollection, TEventPublisher eventPublisher)
            where TEventPublisher : class, IEventPublisher
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(eventPublisher, nameof(eventPublisher));

            serviceCollection.RemoveAll<TEventPublisher>();
            serviceCollection.TryAddSingleton<TEventPublisher>(eventPublisher);
            return serviceCollection;
        }

        /// <summary>
        /// 添加事件发布者依赖
        /// </summary>
        /// <typeparam name="TEventPublisher"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="eventPublisherFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventPublisher<TEventPublisher>(this IServiceCollection serviceCollection, Func<IServiceProvider, TEventPublisher> eventPublisherFactory)
            where TEventPublisher : class, IEventPublisher
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(eventPublisherFactory, nameof(eventPublisherFactory));

            serviceCollection.RemoveAll<TEventPublisher>();
            serviceCollection.TryAddSingleton<TEventPublisher>(eventPublisherFactory);
            return serviceCollection;
        }

        /// <summary>
        /// 添加事件订阅者依赖
        /// </summary>
        /// <typeparam name="TEventPublisher"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="eventSubscriber"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventSubscriber<TEventSubscriber>(this IServiceCollection serviceCollection, TEventSubscriber eventSubscriber)
            where TEventSubscriber : class, IEventSubscriber
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(eventSubscriber, nameof(eventSubscriber));

            serviceCollection.RemoveAll<TEventSubscriber>();
            serviceCollection.TryAddSingleton<TEventSubscriber>(eventSubscriber);
            return serviceCollection;
        }

        /// <summary>
        /// 添加事件订阅者依赖
        /// </summary>
        /// <typeparam name="TEventPublisher"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="eventPublisherFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventSubscriber<TEventSubscriber>(this IServiceCollection serviceCollection, Func<IServiceProvider, TEventSubscriber> eventSubscriberFactory)
            where TEventSubscriber : class, IEventSubscriber
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(eventSubscriberFactory, nameof(eventSubscriberFactory));

            serviceCollection.RemoveAll<TEventSubscriber>();
            serviceCollection.TryAddSingleton<TEventSubscriber>(eventSubscriberFactory);
            return serviceCollection;
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEventBus"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection Subscriber<TEventBus>(this IServiceCollection serviceCollection, Action<IServiceProvider, TEventBus> subscriberAction)
            where TEventBus: class, IEventBus
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(subscriberAction, nameof(subscriberAction));

            var eventBusDescriptor = serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(TEventBus));
            if (eventBusDescriptor == null)
                return serviceCollection;

            serviceCollection.RemoveAll<TEventBus>();
            if (eventBusDescriptor.ImplementationInstance != null)
            {
                var implInstance = eventBusDescriptor.ImplementationInstance as TEventBus;
                serviceCollection.TryAddSingleton<TEventBus>(service =>
                 {
                     subscriberAction(service, implInstance);
                     return implInstance;
                 });
            }
            else if (eventBusDescriptor.ImplementationFactory != null)
            {
                var implFactory = eventBusDescriptor.ImplementationFactory;
                serviceCollection.TryAddSingleton<TEventBus>(service =>
                {
                    var implInstance = implFactory(service) as TEventBus;
                    subscriberAction(service, implInstance);
                    return implInstance;
                });
            }

            return serviceCollection;
        }
    }
}
