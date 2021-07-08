using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;
using Polly.Retry;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

using Rye.EventBus.Abstractions;
using Rye.EventBus.RabbitMQ.Connect;
using Rye.EventBus.RabbitMQ.Internal;
using Rye.EventBus.RabbitMQ.Options;
using Rye.Logger;
using Rye.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ
{
    public class RabbitMQEventBus : IRabbitMQEventBus
    {
        private readonly string _exchange;

        private readonly string _queue;

        private IRabbitMQPersistentConnection _connection;

        private IModel _consumerChannel;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly InternalRabbitMQEventHandler _handler;
        private readonly int _retryCount = 3;

        private readonly ILogger<RabbitMQEventBus> _logger;

        public Func<IEvent, RabbitMQEventPublishContext, Task> OnProducing { get; set; }

        public Func<IEvent, RabbitMQEventPublishContext, Task> OnProduced { get; set; }

        public Func<IEvent, RabbitMQEventPublishErrorContext, Task> OnProductError { get; set; }

        public Func<IEvent, RabbitMQEventSubscribeContext, Task> OnConsuming { get; set; }

        public Func<IEvent, RabbitMQEventSubscribeContext, Task> OnConsumed { get; set; }

        public Func<IEvent, RabbitMQEventSubscribeErrorContext, Task> OnConsumeError { get; set; }

        public IRabbitMQPersistentConnection Connection => _connection;

        public RabbitMQEventBus(IOptions<RabbitMQEventBusOptions> options, IServiceScopeFactory scopeFactory,
            ILoggerFactory loggerFactory)
        {
            Check.NotNull(options, nameof(options));

            var busOptions = options.Value;

            Check.NotNull(busOptions.ConnectionFactory, nameof(busOptions.ConnectionFactory));

            _serviceScopeFactory = scopeFactory;
            _logger = loggerFactory.CreateLogger<RabbitMQEventBus>();
            _exchange = string.IsNullOrEmpty(busOptions.Exchange) ? "RyeEventBus" : busOptions.Exchange;
            _queue = string.IsNullOrEmpty(busOptions.Queue) ? "RyeQueue" : busOptions.Queue;
            _handler = new InternalRabbitMQEventHandler();
            _handler.OnConsumeEvent += OnConsumeEvent;

            _connection = new RabbitMQPersistentConnection(busOptions.ConnectionFactory,
                loggerFactory.CreateLogger<RabbitMQPersistentConnection>(),
                busOptions.RetryCount);

            _consumerChannel = CreateConsumerChannel();
            if (busOptions.OnProducing != null)
                OnProducing = busOptions.OnProducing;
            if (busOptions.OnProduced != null)
                OnProduced = busOptions.OnProduced;
            if (busOptions.OnProductError != null)
                OnProductError = busOptions.OnProductError;
            if (busOptions.OnConsuming != null)
                OnConsuming = busOptions.OnConsuming;
            if (busOptions.OnConsumed != null)
                OnConsumed = busOptions.OnConsumed;
            if (busOptions.OnConsumeError != null)
                OnConsumeError = busOptions.OnConsumeError;
        }

        public Task PublishAsync(string eventRoute, IEvent @event)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            return PublishAsync(eventRoute, @event, 0);
        }

        public Task<bool> PublishForWaitAsync(string eventRoute, IEvent @event)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            return PublishForWaitAsync(eventRoute, @event, 0);
        }

        public Task RetryEvent(IEvent @event, EventContext context)
        {
            return PublishAsync(context.RouteKey, @event, context.RetryCount + 1);
        }


        private async Task PublishAsync(string routeKey, IEvent @event, int retryCount = 0)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogInformation($"Could not publish event: {@event.EventId} after {time.TotalSeconds:n1}s ({ex.Message})");
                });

            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _exchange, type: "topic");

                var message = @event.ToJsonString();
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent
                properties.Headers["retry-count"] = retryCount;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var context = new RabbitMQEventPublishContext
                        {
                            ServiceProvider = scope.ServiceProvider,
                            EventBus = this,
                            RouteKey = routeKey,
                            RetryCount = retryCount,
                            Exchange = _exchange,
                            Queue = _queue,
                            BasicProperties = properties,
                        };

                        if (OnProducing != null)
                            await OnProducing(@event, context);

                        policy.Execute(() =>
                        {
                            channel.BasicPublish(
                                    exchange: _exchange,
                                    routingKey: routeKey,
                                    mandatory: true,
                                    basicProperties: properties,
                                    body: body);
                        });

                        if (OnProduced != null)
                            await OnProduced(@event, context);

                    }
                    catch (Exception ex)
                    {
                        if (OnProductError == null)
                            throw;

                        var errorContext = new RabbitMQEventPublishErrorContext
                        {
                            ServiceProvider = scope.ServiceProvider,
                            EventBus = this,
                            RouteKey = routeKey,
                            RetryCount = retryCount,
                            Exception = ex,
                            Exchange = _exchange,
                            Queue = _queue,
                            BasicProperties = properties
                        };
                        await OnProductError(@event, errorContext);
                    }
                }
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange,
                                    type: "direct");

            channel.QueueDeclare(queue: _queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }


        private async Task<bool> PublishForWaitAsync(string routeKey, IEvent @event, int retryCount = 0)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogInformation($"Could not publish event: {@event.EventId} after {time.TotalSeconds:n1}s ({ex.Message})");
                });

            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _exchange, type: "topic");

                var message = @event.ToJsonString();
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent
                properties.Headers["retry-count"] = retryCount;

                channel.ConfirmSelect(); // 开启发布确认
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var context = new RabbitMQEventPublishContext
                        {
                            ServiceProvider = scope.ServiceProvider,
                            EventBus = this,
                            RouteKey = routeKey,
                            RetryCount = retryCount,
                            Exchange = _exchange,
                            Queue = _queue,
                            BasicProperties = properties,
                        };

                        if (OnProducing != null)
                            await OnProducing(@event, context);

                        policy.Execute(() =>
                        {
                            channel.BasicPublish(
                                    exchange: _exchange,
                                    routingKey: routeKey,
                                    mandatory: true,
                                    basicProperties: properties,
                                    body: body);
                        });

                        if (OnProduced != null)
                            await OnProduced(@event, context);

                        return channel.WaitForConfirms();
                    }
                    catch (Exception ex)
                    {
                        if (OnProductError == null)
                            throw;

                        var errorContext = new RabbitMQEventPublishErrorContext
                        {
                            ServiceProvider = scope.ServiceProvider,
                            EventBus = this,
                            RouteKey = routeKey,
                            RetryCount = retryCount,
                            Exception = ex,
                            Exchange = _exchange,
                            Queue = _queue,
                            BasicProperties = properties
                        };
                        await OnProductError(@event, errorContext);

                        return false;
                    }
                }
            }
        }

        public void Subscribe(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            _handler.AddHandlers(eventRoute, handlers);
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queue,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogTrace("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                var retryCount = 0;
                if (eventArgs.BasicProperties.Headers.TryGetValue("retry-count", out var str) && str != null)
                {
                    retryCount = str.ParseByInt();
                }

                var wrapper = new EventWrapper
                {
                    Exchange = _exchange,
                    Queue = _queue,
                    RouteKey = eventArgs.RoutingKey,
                    Event = message,
                    RetryCount = retryCount
                };

                await _handler.OnEvent(wrapper, eventArgs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR Processing message \"{message}\", exception: {ex.Message}");
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task OnConsumeEvent(BasicDeliverEventArgs eventArgs, EventWrapper wrapper, List<IEventHandler> handlers)
        {
            IEvent firstEvent = null;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var context = new RabbitMQEventSubscribeContext
                    {
                        EventBus = this,
                        ServiceProvider = scope.ServiceProvider,
                        Exchange = _exchange,
                        Queue = _queue,
                        RouteKey = eventArgs.RoutingKey,
                        Ack = false,
                    };
                    Type eventType;

                    var eventTypeDict = new Dictionary<Type, IEvent>();
                    for (var i = 0; i < handlers.Count; i++)
                    {
                        eventType = handlers[0].GetEventType();
                        IEvent @event;
                        if (!eventTypeDict.ContainsKey(eventType))
                        {
                            @event = wrapper.Event.ToObject(eventType) as IEvent;
                            eventTypeDict.Add(eventType, @event);
                        }
                        else
                        {
                            @event = eventTypeDict[eventType];
                        }

                        if (i == 0)
                        {
                            firstEvent = @event;
                            if (OnConsuming != null)
                                await OnConsuming(firstEvent, context);
                        }

                        await handlers[i].OnEvent(@event, context);
                    }

                    if (OnConsumed != null && firstEvent != null)
                    {
                        await OnConsumed(firstEvent, context);
                    }

                    context.Ack = true;
                    if (context.Ack)
                    {
                        _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    else
                    {
                        _consumerChannel.BasicNack(eventArgs.DeliveryTag, false, false);
                    }

                }
                catch (Exception ex)
                {
                    if (OnConsumeError == null || firstEvent == null)
                    {
                        _consumerChannel.BasicNack(eventArgs.DeliveryTag, false, false);
                        throw;
                    }

                    var context = new RabbitMQEventSubscribeErrorContext(_consumerChannel, eventArgs)
                    {
                        Exchange = eventArgs.Exchange,
                        Queue = _queue,
                        RouteKey = eventArgs.RoutingKey,
                        ServiceProvider = scope.ServiceProvider,
                        Exception = ex,
                        Ack = false,
                    };
                    await OnConsumeError(firstEvent, context);

                    if (context.Ack)
                    {
                        _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    else
                    {
                        _consumerChannel.BasicNack(eventArgs.DeliveryTag, false, false);
                    }
                }
            }
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _consumerChannel?.Dispose();
                    _connection?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
