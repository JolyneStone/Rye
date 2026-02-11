using Rye.EventBus.Abstractions;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQEventSubscriber: IEventSubscriber
    {
        IRabbitMQPersistentConnection Connection { get; }

        Task Subscribe(string exchange, string queue, string eventRoute, IEnumerable<IEventHandler> handlers);
    }
}
