using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQEventPublisher: IEventPublisher
    {
        IRabbitMQPersistentConnection Connection { get; }

        Task<bool> PublishForWaitAsync(string eventRoute, IEvent @event);

        Task<bool> PublishForWaitAsync(string exchange, string queue, string eventRoute, IEvent @event);

        Task PublishAsync(string exchange, string queue, string eventRoute, IEvent @event);
    }
}
