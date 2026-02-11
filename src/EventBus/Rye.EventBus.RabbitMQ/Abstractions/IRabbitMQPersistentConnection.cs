using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rye.EventBus.RabbitMQ.Event;
using System;
using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection: IDisposable
    {
        bool IsConnected { get; }

        Task<bool> TryConnectAsync();

        Task<IChannel> CreateChannelAsync();

        event AsyncEventHandler<ConnectionEventArgs> OnConnection;
    }
}
