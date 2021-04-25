using System;

namespace Rye.EventBus.Abstractions
{
    public abstract class EventContext
    {
        public IEventBus EventBus { get; init; }
        public IServiceProvider ServiceProvider { get; init; }
        public string RouteKey { get; init; }
        public int RetryCount { get; set; }
    }
}
