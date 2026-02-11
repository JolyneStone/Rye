
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using Polly;
using Polly.Retry;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Rye.EventBus.RabbitMQ.Event;
using Rye.Logger;

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ.Connect
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private IConnection _connection;
        private bool _disposed;
        private readonly AsyncLock sync_root = new AsyncLock();
        private Task _loopTask;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory,
            ILogger<RabbitMQPersistentConnection> logger,
            int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _retryCount = retryCount;
            _logger = logger;
        }

        public event AsyncEventHandler<ConnectionEventArgs> OnConnection;

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public Task<IChannel> CreateChannelAsync()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateChannelAsync(new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: false));
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (_loopTask != null)
                {
                    _loopTask.Dispose();
                    _loopTask = null;
                }
                _connection.Dispose();
                _disposed = true;
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public async Task<bool> TryConnectAsync()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            if (IsConnected) return true;

            using (await sync_root.LockAsync()) 
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning($"RabbitMQ Client could not connect after {time.TotalSeconds:n1}s ({ex.Message})");
                    }
                );

                await policy.ExecuteAsync(async () =>
                {
                    _connection = await _connectionFactory
                          .CreateConnectionAsync();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdownAsync += OnConnectionShutdown;
                    _connection.CallbackExceptionAsync += OnCallbackException;
                    _connection.ConnectionBlockedAsync += OnConnectionBlocked;

                    OnConnection?.Invoke(this, new ConnectionEventArgs(_connection));
                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    LoopTryConnect();
                    return false;
                }
            }
        }

        private void LoopTryConnect()
        {
            if (_loopTask != null)
            {
                _loopTask = Task.Run(async () =>
                {
                    while (!await TryConnectAsync())
                    {
                        await Task.Delay(5000);
                    }

                    _loopTask = null;
                });
            }
        }

        private Task OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return Task.CompletedTask;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            return TryConnectAsync();
        }

        Task OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return Task.CompletedTask;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            return TryConnectAsync();
        }

        Task OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return Task.CompletedTask;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            return TryConnectAsync();
        }
    }
}
