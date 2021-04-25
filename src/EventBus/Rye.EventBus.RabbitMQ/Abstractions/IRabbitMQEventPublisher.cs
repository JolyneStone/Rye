using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQEventPublisher: IEventPublisher
    {
        IRabbitMQPersistentConnection Connection { get; }

        Task<bool> PublishForWaitAsync(string eventRoute, IEvent @event);
    }
}
