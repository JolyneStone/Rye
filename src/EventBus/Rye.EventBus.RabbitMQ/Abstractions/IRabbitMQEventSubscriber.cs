using Rye.EventBus.Abstractions;

using System.Collections.Generic;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQEventSubscriber: IEventSubscriber
    {
        IRabbitMQPersistentConnection Connection { get; }

        void Subscribe(string exchange, string queue, string eventRoute, IEnumerable<IEventHandler> handlers);
    }
}
