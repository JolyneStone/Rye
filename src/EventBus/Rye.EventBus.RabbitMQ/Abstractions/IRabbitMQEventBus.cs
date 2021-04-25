using Rye.EventBus.Abstractions;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQEventBus: IEventBus, IRabbitMQEventPublisher, IRabbitMQEventSubscriber
    {
    }
}
