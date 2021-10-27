using RabbitMQ.Client;

using System;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection: IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        event EventHandler<IConnection> OnConnection;
    }
}
