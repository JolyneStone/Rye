using CSRedis;

using Rye.Cache.Redis.Builder;
using Rye.Cache.Redis.Options;
using Rye.EventBus.Abstractions;
using Rye.EventBus.Redis.Internal;
using Rye.EventBus.Redis.Options;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static CSRedis.CSRedisClient;

namespace Rye.EventBus.Redis
{
    public class RedisEventBus : IRedisEventBus
    {
        private readonly CSRedisClient _redisClient;
        private readonly InternalRedisEventHandler _handler;
        private readonly string _key;
        private readonly SubscribeObject _subscribeObject;

        public RedisEventBus(RedisEventBusOptions options, IServiceProvider serviceProvider)
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


            _key = string.IsNullOrEmpty(options.Key) ? "RyeEventBus" : options.Key;
            //var clientId = options.ClientId;
            _handler = new InternalRedisEventHandler(_key, this, serviceProvider);
            _subscribeObject = _redisClient.Subscribe((_key, msg => RedisSubscribe(msg.Body)));
        }

        private async void RedisSubscribe(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            await _handler.OnEvent(message.ToObject<EventWrapper>());
        }

        public void Publish(string eventRoute, IEvent @event)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            _redisClient.Publish(_key, new EventWrapper
            {
                Route = eventRoute,
                Event = @event.ToJsonString()
            }.ToJsonString());
        }

        public Task PublishAsync(string eventRoute, IEvent @event)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            return _redisClient.PublishAsync(_key, new EventWrapper
            {
                Route = eventRoute,
                Event = @event.ToJsonString()
            }.ToJsonString());
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
                    _subscribeObject?.Dispose();
                    _redisClient?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        #endregion
    }
}
