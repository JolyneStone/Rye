using CSRedis;

using Microsoft.Extensions.DependencyInjection;

using Rye.Cache.Redis.Builder;
using Rye.Cache.Redis.Options;
using Rye.EventBus.Abstractions;
using Rye.EventBus.Redis.Internal;
using Rye.EventBus.Redis.Options;
using Rye.Util;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.EventBus.Redis
{
    public class RedisEventBus : IRedisEventBus
    {
        private readonly string _key;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CSRedisClient _redisClient;
        private readonly InternalRedisEventHandler _handler;
        //private readonly SubscribeListBroadcastObject _subscribeObject;

        public event Func<IEvent, RedisEventContext, Task> OnProducing;

        public event Func<IEvent, RedisEventContext, Task> OnProduced;

        public event Func<IEvent, RedisEventErrorContext, Task> OnProductError;

        public event Func<IEvent, RedisEventContext, Task> OnConsuming;

        public event Func<IEvent, RedisEventContext, Task> OnConsumed;

        public event Func<IEvent, RedisEventErrorContext, Task> OnConsumeError;

        public RedisEventBus(RedisEventBusOptions options, IServiceScopeFactory scopeFactory)
        {
            Check.NotNull(options, nameof(options));
            //Check.NotNullOrEmpty(options.ClientId, nameof(options.ClientId));
            if (options.RedisClient != null)
            {
                _redisClient = options.RedisClient;
            }
            else if (options.RedisOptions != null)
            {
                var redisOptions = new RedisOptions();
                options.RedisOptions(redisOptions);

                _redisClient = new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(redisOptions), redisOptions.Sentinels, redisOptions.ReadOnly);
            }
            else
            {
                _redisClient = RedisHelper.Instance;
            }

            _serviceScopeFactory = scopeFactory;
            _key = string.IsNullOrEmpty(options.Key) ? "RyeEventBus" : options.Key;
            var clientId = options.ClientId;
            _handler = new InternalRedisEventHandler();
            _handler.OnConsumeEvent += OnConsumeEvent;

            if (options.OnProducing != null)
                OnProducing = options.OnProducing;
            if (options.OnProduced != null)
                OnProduced = options.OnProduced;
            if (options.OnProductError != null)
                OnProductError = options.OnProductError;
            if (options.OnConsuming != null)
                OnConsuming = options.OnConsuming;
            if (options.OnConsumed != null)
                OnConsumed = options.OnConsumed;
            if (options.OnConsumeError != null)
                OnConsumeError = options.OnConsumeError;

            _redisClient.SubscribeListBroadcast(_key, clientId, RedisSubscribe);
        }

        private async Task OnConsumeEvent(EventWrapper wrapper, List<IEventHandler> handlers)
        {
            IEvent firstEvent = null;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var context = new RedisEventContext
                    {
                        Key = _key,
                        EventBus = this,
                        ServiceProvider = scope.ServiceProvider,
                        RouteKey = wrapper.Route,
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
                }
                catch (Exception ex)
                {
                    if (OnConsumeError == null || firstEvent == null)
                        throw;

                    var context = new RedisEventErrorContext
                    {
                        EventBus = this,
                        ServiceProvider = scope.ServiceProvider,
                        RouteKey = wrapper.Route,
                        RetryCount = wrapper.RetryCount,
                        Exception = ex
                    };
                    await OnConsumeError(firstEvent, context);
                }
            }
        }

        private async void RedisSubscribe(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            await _handler.OnEvent(message.ToObject<EventWrapper>());
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

        public Task RetryEvent(IEvent @event, EventContext context)
        {
            return PublishAsync(context.RouteKey, @event, context.RetryCount + 1);
        }

        private async Task PublishAsync(string route, IEvent @event, int retryCount = 0)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var context = new RedisEventContext
                    {
                        ServiceProvider = scope.ServiceProvider,
                        EventBus = this,
                        RouteKey = route,
                        RetryCount = retryCount
                    };

                    if (OnProducing != null)
                        await OnProducing(@event, context);

                    await _redisClient.LPushAsync(_key, new EventWrapper
                    {
                        Route = route,
                        Event = @event.ToJsonString(),
                        RetryCount = retryCount
                    });

                    if (OnProduced != null)
                        await OnProduced(@event, context);
                }
                catch (Exception ex)
                {
                    if (OnProductError == null)
                        throw;

                    var context = new RedisEventErrorContext
                    {
                        ServiceProvider = scope.ServiceProvider,
                        EventBus = this,
                        RouteKey = route,
                        RetryCount = retryCount,
                        Exception = ex
                    };
                    await OnProductError(@event, context);
                }
            }
        }

        public void Subscribe(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            _handler.AddHandlers(eventRoute, handlers);
        }

        #region Dispose

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //_subscribeObject?.Dispose(); // 继续订阅，待下次启动后可以读取未处理的消息
                    _redisClient?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
