using DotNetCore.CAP;

using Rye.EventBus.Abstractions;

namespace Rye.EventBus.RabbitMQ
{
    public class CapEventPublishErrorContext : EventErrorContext
    {
        public CapHeader Header { get; init; }
    }

    //public class CapEventSubscribeErrorContext : EventErrorContext
    //{
    //    public CapHeader Header { get; init; }
    //}
}
