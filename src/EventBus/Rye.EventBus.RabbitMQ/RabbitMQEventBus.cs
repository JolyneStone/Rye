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
using System.Collections.Concurrent;
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

        //private IModel _consumerChannel;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly InternalRabbitMQEventHandler _handler;
        private readonly int _retryCount = 3;

        private readonly ILogger<RabbitMQEventBus> _logger;
        private ConcurrentDictionary<string, IModel> _channelDict = new ConcurrentDictionary<string, IModel>();
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

            //_consumerChannel = CreateConsumerChannel();
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
            return PublishAsync(_exchange, _queue, eventRoute, @event);
        }

        public Task PublishAsync(string exchange, string queue, string eventRoute, IEvent @event)
        {
            Check.NotNull(exchange, nameof(exchange));
            Check.NotNull(queue, nameof(queue));
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            return PublishAsync(exchange, queue, eventRoute, @event, 0);
        }

        public Task<bool> PublishForWaitAsync(string eventRoute, IEvent @event)
        {
            return PublishForWaitAsync(_exchange, _queue, eventRoute, @event);
        }

        public Task<bool> PublishForWaitAsync(string exchange, string queue, string eventRoute, IEvent @event)
        {
            Check.NotNull(exchange, nameof(exchange));
            Check.NotNull(queue, nameof(queue));
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            return PublishForWaitAsync(exchange, queue, eventRoute, @event, 0);
        }

        public Task RetryEvent(IEvent @event, EventContext context)
        {
            if (context is RabbitMQEventPublishContext pubContext)
                return PublishAsync(pubContext.Exchange, pubContext.Queue, context.RouteKey, @event, context.RetryCount + 1);

            if (context is RabbitMQEventSubscribeContext subContext)
                return PublishAsync(subContext.Exchange, subContext.Queue, context.RouteKey, @event, context.RetryCount + 1);

            return Task.CompletedTask;
        }


        private async Task PublishAsync(string exchange, string queue, string routeKey, IEvent @event, int retryCount = 0)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            if(_channelDict.TryGetValue($"{exchange}:{queue}", out var channel))
            {
                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogInformation($"Could not publish event: {@event.EventId} after {time.TotalSeconds:n1}s ({ex.Message})");
                });

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
                            Exchange = exchange,
                            Queue = queue,
                            BasicProperties = properties,
                        };

                        if (OnProducing != null)
                            await OnProducing(@event, context);

                        policy.Execute(() =>
                        {
                            channel.BasicPublish(
                                    exchange: exchange,
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
                            Exchange = exchange,
                            Queue = queue,
                            BasicProperties = properties
                        };
                        await OnProductError(@event, errorContext);
                    }
                }
            }
        }

        private IModel CreateConsumerChannel(string exchange, string queue, string routeKey)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchange,
                                    type: ExchangeType.Direct,
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // 绑定路由键
            channel.QueueBind(queue, exchange, routeKey);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                var key = $"{exchange}:{queue}";
                if (_channelDict.TryGetValue(key, out var oldChannel))
                {
                    oldChannel.Dispose();
                    var newChannel = CreateConsumerChannel(exchange, queue, routeKey);
                    _channelDict.TryUpdate(key, newChannel, oldChannel);
                    StartBasicConsume(newChannel, queue);
                }
            };

            return channel;
        }


        private async Task<bool> PublishForWaitAsync(string exchange, string queue, string routeKey, IEvent @event, int retryCount = 0)
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

            var message = @event.ToJsonString();
            var body = Encoding.UTF8.GetBytes(message);

            if (_channelDict.TryGetValue($"{exchange}:{queue}", out var channel))
            {
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
                            Exchange = exchange,
                            Queue = queue,
                            BasicProperties = properties,
                        };

                        if (OnProducing != null)
                            await OnProducing(@event, context);

                        policy.Execute(() =>
                        {
                            channel.BasicPublish(
                                    exchange: exchange,
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
                            Exchange = exchange,
                            Queue = queue,
                            BasicProperties = properties
                        };
                        await OnProductError(@event, errorContext);

                        return false;
                    }
                }
            }

            return false;
        }

        public void Subscribe(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            _handler.AddHandlers($"{_exchange}:{_queue}:{eventRoute}", handlers);
        }

        public void Subscribe(string exchange, string queue, string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            if (exchange == null)
                exchange = _exchange;
            if (queue == null)
                queue = _queue;

            _handler.AddHandlers($"{exchange}:{queue}:{eventRoute}", handlers);
        }

        private void StartBasicConsume(IModel channel, string queue)
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (channel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.Received += (sender, args) => Consumer_Received(queue).Invoke(sender, args);

                channel.BasicConsume(
                    queue: _queue,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogTrace("StartBasicConsume can't call on _consumerChannel == null");
            }
        }
        private Func<object, BasicDeliverEventArgs, Task> Consumer_Received(string queue)
        {
            return async (object sender, BasicDeliverEventArgs eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
                var wrapper = new EventWrapper
                {
                    Key = $"{eventArgs.Exchange}:{queue}:{eventArgs.RoutingKey}",
                    Exchange = eventArgs.Exchange,
                    Queue = queue,
                    RouteKey = eventArgs.RoutingKey,
                    Event = message,
                };

                try
                {
                    await _handler.OnEvent(wrapper, eventArgs);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"ERROR Processing message \"{message}\", exception: {ex.Message}");
                    if (_channelDict.TryGetValue($"{eventArgs.Exchange}:{queue}", out var channel))
                    {
                        channel.BasicNack(eventArgs.DeliveryTag, false, false);
                    }
                }
            };
        }

        private async Task OnConsumeEvent(BasicDeliverEventArgs eventArgs, EventWrapper wrapper, List<IEventHandler> handlers)
        {
            IEvent firstEvent = null;
            if (!_channelDict.TryGetValue($"{eventArgs.Exchange}:{wrapper.Queue}", out var channel))
                return;

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
                        Ack = true,
                    };

                    Type eventType = null;
                    var eventTypeDict = new Dictionary<Type, IEvent>();
                    for (var i = 0; i < handlers.Count; i++)
                    {
                        if (eventType == null)
                        {
                            eventType = handlers[i].GetEventType();
                            if (eventType == null)
                            {
                                continue;
                            }
                        }

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


                    if (context.Ack)
                    {
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(eventArgs.DeliveryTag, false, false);
                    }

                }
                catch (Exception ex)
                {
                    if (OnConsumeError == null || firstEvent == null)
                    {
                        channel.BasicNack(eventArgs.DeliveryTag, false, false);
                        throw;
                    }

                    var context = new RabbitMQEventSubscribeErrorContext(channel, eventArgs)
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
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(eventArgs.DeliveryTag, false, false);
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
                    if (_channelDict != null)
                    {
                        foreach (var channel in _channelDict.Values)
                            channel?.Dispose();
                    }
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
