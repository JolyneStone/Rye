using Rye.EventBus.Abstractions;

namespace Rye.EventBus.RabbitMQ.Abstractions
{
    public abstract class RabbitMQEventHandler<TEvent> : EventHandler<TEvent, RabbitMQEventPublishContext>
    where TEvent : IEvent
    {
    }
}
