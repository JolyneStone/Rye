using System;

namespace Rye.EventBus.Abstractions
{
    public abstract class EventContext
    {
        public IEventBus EventBus { get; init; }
        public IServiceProvider ServiceProvider { get; init; }
        public string EventRoute { get; init; }
    }
}
