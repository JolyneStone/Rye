using Rye.EventBus.Abstractions;

namespace Rye.EventBus.CAP.Abstractions
{
    public abstract class CapEventHandler<TEvent> : EventHandler<TEvent, CapEventPublishContext>
        where TEvent : IEvent
    {
    }
}
