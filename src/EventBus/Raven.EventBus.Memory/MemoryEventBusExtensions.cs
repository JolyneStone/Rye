using Raven.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Raven.EventBus.Memory
{
    public static class MemoryEventBusExtensions
    {
        public static void AddHandler<TEvent, THandler>(this IEventSubscriber subscriber, THandler handler)
            where TEvent : class, IEvent<TEvent>, new()
            where THandler : MemoryEventHandler<TEvent>
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            subscriber.AddHandlers<TEvent, THandler>(new THandler[] { handler });
        }

        public static void AddHandlers<TEvent, THandler>(this IEventSubscriber subscriber, IEnumerable<THandler> handlers)
           where TEvent : class, IEvent<TEvent>, new()
           where THandler : MemoryEventHandler<TEvent>
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            if (handlers is null && handlers.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(handlers));
            }

            subscriber.AddHandlers<TEvent, THandler>(handlers);
        }
    }
}
