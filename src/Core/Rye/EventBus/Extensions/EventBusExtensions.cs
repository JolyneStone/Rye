using Rye.EventBus;
using Rye.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye
{
    public static class EventBusExtensions
    {
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

        public static void Publish(this IEventPublisher publisher, IEvent @event)
        {
            Check.NotNull(publisher, nameof(publisher));
            Check.NotNull(@event, nameof(@event));

            publisher.Publish(@event.GetEventRoute(), @event);
        }

        public static Task PublishAsync(this IEventPublisher publisher, IEvent @event)
        {
            Check.NotNull(publisher, nameof(publisher));
            Check.NotNull(@event, nameof(@event));

            return publisher.PublishAsync(@event.GetEventRoute(), @event);
        }
    }
}
