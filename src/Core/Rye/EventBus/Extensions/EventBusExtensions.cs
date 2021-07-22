using Rye.EventBus;
using Rye.EventBus.Abstractions;
using Rye.EventBus.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye
{
    public static class EventBusExtensions
    {
        public static void Subscribe<TEvent, TContext>(this IEventSubscriber subscriber, string eventRoute, Func<TEvent, TContext, Task> func)
            where TEvent: IEvent
            where TContext: EventContext
        {
            Check.NotNull(subscriber, nameof(subscriber));
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(func, nameof(func));

            subscriber.Subscribe(eventRoute, new IEventHandler[] { new InternalEventHandler<TEvent, TContext>(func) });
        }

        public static void Subscribe(this IEventSubscriber subscriber, string eventRoute, IEventHandler handler)
        {
            Check.NotNull(subscriber, nameof(subscriber));
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handler, nameof(handler));

            subscriber.Subscribe(eventRoute, new IEventHandler[] { handler });
        }

        public static void Subscribe<TEvent>(this IEventSubscriber subscriber, IEventHandler handler) 
            where TEvent: IEvent
        {
            Check.NotNull(subscriber, nameof(subscriber));
            Check.NotNull(handler, nameof(handler));

            subscriber.Subscribe(typeof(TEvent).GetEventRoute(), new IEventHandler[] { handler });
        }

        public static void Subscribe<TEvent>(this IEventSubscriber subscriber, IEnumerable<IEventHandler> handlers)
            where TEvent : IEvent
        {
            Check.NotNull(subscriber, nameof(subscriber));
            Check.NotNull(handlers, nameof(handlers));

            subscriber.Subscribe(typeof(TEvent).GetEventRoute(), handlers);
        }

        public static Task PublishAsync(this IEventPublisher publisher, IEvent @event)
        {
            Check.NotNull(publisher, nameof(publisher));
            Check.NotNull(@event, nameof(@event));

            return publisher.PublishAsync(@event.GetEventRoute(), @event);
        }
    }
}
