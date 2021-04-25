
using Polly;
using Polly.Retry;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

using Rye.Logger;

using System;
using System.IO;
using System.Net.Sockets;

namespace Rye.EventBus.RabbitMQ.Connect
{
    public class RabbitMQPersistentConnection: IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private IConnection _connection;
        private bool _disposed;
        private readonly object sync_root = new object();

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _retryCount = retryCount;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                LogRecord.Critical(nameof(RabbitMQPersistentConnection), ex.ToString());
            }
        }

        public bool TryConnect()
        {
            LogRecord.Info(nameof(RabbitMQPersistentConnection), "RabbitMQ Client is trying to connect");

            lock (sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        LogRecord.Warn(nameof(RabbitMQPersistentConnection), $"RabbitMQ Client could not connect after {time.TotalSeconds:n1}s ({ex.Message})");
                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                          .CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                   LogRecord.Info(nameof(RabbitMQPersistentConnection), $"RabbitMQ Client acquired a persistent connection to '{_connection.Endpoint.HostName}' and is subscribed to failure events");

                    return true;
                }
                else
                {
                    LogRecord.Critical(nameof(RabbitMQPersistentConnection), "FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            LogRecord.Warn(nameof(RabbitMQPersistentConnection), "A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            LogRecord.Warn(nameof(RabbitMQPersistentConnection), "A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            LogRecord.Warn(nameof(RabbitMQPersistentConnection), "A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}
