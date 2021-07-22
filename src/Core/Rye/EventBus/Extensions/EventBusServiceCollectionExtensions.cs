using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Internal;
using Rye.EventBus.Lightweight;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Rye.EventBus
{
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 添加事件总线依赖
        /// </summary>
        /// <typeparam name="TEventBus"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus<TIEventBus, TEventBus>(this IServiceCollection serviceCollection)
           where TIEventBus : class, IEventBus
            where TEventBus : class, TIEventBus
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            serviceCollection.RemoveAll<TEventBus>();
            serviceCollection.TryAddSingleton<TIEventBus, TEventBus>();
            return serviceCollection;
        }

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
            where TEventBus : class, IEventBus
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
        /// 配置事件总线
        /// </summary>
        /// <typeparam name="TEventBus"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="subscriberAction"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigBus<TEventBus>(this IServiceCollection serviceCollection, Action<IServiceProvider, TEventBus> subscriberAction)
            where TEventBus : class, IEventBus
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

        /// <summary>
        /// 应用事件订阅
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceProvider ApplySubscriberHandler<TSubscriber, TAttribute>(this IServiceProvider services,
            Action<TSubscriber, TAttribute, IEventHandler> action)
            where TSubscriber : IEventSubscriber
            where TAttribute : Attribute
        {
            // 查找所有贴了 [SubscribeMessage] 特性的方法，并且含有两个参数，第一个参数为 string eventId，第二个参数为 T payload
            var handlerType = typeof(ISubscribeEventHandler);
            var attributeType = typeof(TAttribute);
            var typeMethods = App.ScanTypes
                    // 查询符合条件的订阅类型
                    .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract && handlerType.IsAssignableFrom(u))
                    // 查询符合条件的订阅方法
                    .SelectMany(u =>
                        u.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                         .Where(m =>
                         {
                             if (!m.IsDefined(attributeType, false))
                                 return false;

                             var p = m.GetParameters();
                             return p.Length == 2 &&
                                    typeof(IEvent).IsAssignableFrom(p[0].ParameterType) &&
                                    typeof(EventContext).IsAssignableFrom(p[1].ParameterType);
                         })
                         .GroupBy(m => m.DeclaringType));

            if (!typeMethods.Any()) return services;

            var subscribeMethod = typeof(EventBusServiceCollectionExtensions).GetMethod("Subscribe", BindingFlags.NonPublic | BindingFlags.Static);

            using (var scope = services.CreateScope())
            {
                var sub = scope.ServiceProvider.GetRequiredService<TSubscriber>();
                // 遍历所有订阅类型
                foreach (var item in typeMethods)
                {
                    if (!item.Any()) continue;

                    foreach (var method in item)
                    {
                        var p = method.GetParameters();
                        var parameterTypes = new Type[]
                        {
                            typeof(TSubscriber),
                            typeof(TAttribute),
                            p[0].ParameterType,
                            p[1].ParameterType
                        };
                        subscribeMethod.MakeGenericMethod(parameterTypes)
                            .Invoke(null, new object[] { sub, item.Key, method, action });
                    }
                }
            }

            return services;
        }

        private static void Subscribe<TSubscriber, TAttribute, TEvent, TContext>(
            TSubscriber subscriber,
            Type type,
            MethodInfo method,
            Action<TSubscriber, TAttribute, IEventHandler> action)
            where TSubscriber : IEventSubscriber
            where TAttribute : Attribute
            where TEvent : IEvent
            where TContext : EventContext
        {
            var handler = CallSubscribeExpression<TEvent, TContext>(type, method);

            // 获取所有消息特性
            var subscribeMessageAttributes = method.GetCustomAttributes<TAttribute>();

            // 注册订阅
            foreach (var subscribeMessageAttribute in subscribeMessageAttributes)
            {
                action(subscriber, subscribeMessageAttribute, new InternalEventHandler<TEvent, TContext>(handler));
                //subscriber.Subscribe(
                //    subscribeMessageAttribute.Exchange,
                //    subscribeMessageAttribute.Queue,
                //    subscribeMessageAttribute.RouteKey,
                //    handler);
            }
        }

        /// <summary>
        /// 生成调用订阅者表达式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static Func<TEvent, TContext, Task> CallSubscribeExpression<TEvent, TContext>(Type type, MethodInfo method)
        {
            var paramHandlerExpression = Expression.Parameter(typeof(object));
            var paramEventExpression = Expression.Parameter(typeof(TEvent));
            var paramContextExpression = Expression.Parameter(typeof(TContext));

            var convertExpression = Expression.Convert(paramHandlerExpression, type);
            var callExpression = Expression.Call(
                convertExpression,
                method,
                paramEventExpression,
                paramContextExpression);


            // 对返回Task或Task<>的方法进行处理
            if (method.ReturnType != null && typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                var lambdaExpression = Expression.Lambda<Func<object, TEvent, TContext, Task>>(
                   callExpression,
                   paramHandlerExpression,
                   paramEventExpression,
                   paramContextExpression);

                var lambda = lambdaExpression.Compile();

                return async (s, o) =>
                {
                    using (var scope = App.GetRequiredService<IServiceScopeFactory>()
                            .CreateScope())
                    {
                        await lambda(scope.ServiceProvider.GetRequiredService(type), s, o);
                    }
                };
            }
            // 对返回ValueTask或ValueTask<>的方法进行处理
            else if (method.ReturnType != null && typeof(ValueTask).IsAssignableFrom(method.ReturnType))
            {
                var lambdaExpression = Expression.Lambda<Func<object, TEvent, TContext, ValueTask>>(
                   callExpression,
                   paramHandlerExpression,
                   paramEventExpression,
                   paramContextExpression);

                var lambda = lambdaExpression.Compile();

                return async (s, o) =>
                {
                    using (var scope = App.GetRequiredService<IServiceScopeFactory>()
                            .CreateScope())
                    {
                        await lambda(scope.ServiceProvider.GetRequiredService(type), s, o);
                    }
                };
            }
            // 对无返回值或返回普通类型的方法进行处理
            else
            {
                var lambdaExpression = Expression.Lambda<Action<object, TEvent, TContext>>(
                    callExpression,
                    paramHandlerExpression,
                    paramEventExpression,
                    paramContextExpression);

                var lambda = lambdaExpression.Compile();

                return (s, o) =>
                {
                    using (var scope = App.GetRequiredService<IServiceScopeFactory>()
                            .CreateScope())
                    {
                        lambda(scope.ServiceProvider.GetRequiredService(type), s, o);
                        return Task.CompletedTask;
                    }
                };
            }
        }
    }
}
