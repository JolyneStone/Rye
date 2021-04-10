using System;
using System.Collections.Generic;

namespace Rye.EventBus.Abstractions
{
    public interface IEventSubscriber : IDisposable
    {
        void Subscribe(string eventRoute, IEnumerable<IEventHandler> handlers);
    }
}
