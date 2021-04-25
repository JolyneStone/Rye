using Rye.EventBus.Abstractions;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQEventSubscriber: IEventSubscriber
    {
        IRabbitMQPersistentConnection Connection { get; }
    }
}
