using System;
using System.Collections.Generic;

namespace Raven.EventBus.Abstractions
{
    public interface IEventSubscriber : IDisposable
    {
        void Subscribe();

        void AddHandlers<TEvent, THandler>(IEnumerable<THandler> handlers)
           where TEvent : class, IEvent<TEvent>, new()
           where THandler : IEventHandler<TEvent>;
    }
}
