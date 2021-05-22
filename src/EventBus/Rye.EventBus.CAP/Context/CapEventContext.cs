using DotNetCore.CAP;

using Rye.EventBus.Abstractions;

namespace Rye.EventBus.CAP
{
    public class CapEventPublishContext : EventContext
    {
        public CapHeader Header { get; init; }
    }

    //public class CapEventSubscribeContext: EventContext
    //{
    //    public CapHeader Header { get; init; }
    //}
}
