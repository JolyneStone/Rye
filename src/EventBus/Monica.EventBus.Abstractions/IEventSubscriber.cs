using System;
using System.Collections.Generic;

namespace Monica.EventBus.Abstractions
{
    public interface IEventSubscriber : IDisposable
    {
        void Subscribe();

        void AddHandlers<TEvent, THandler>(string eventName, IEnumerable<THandler> handlers)
           where TEvent : class, IEvent<TEvent>, new()
           where THandler : IEventHandler<TEvent>;
    }
}
