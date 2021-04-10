using Rye.EventBus.Abstractions;
using Rye.Logger;
using Rye.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rye.EventBus.Redis.Internal
{
    internal class InternalRedisEventHandler
    {
        private readonly Dictionary<string, List<IEventHandler>> _handler = new Dictionary<string, List<IEventHandler>>();
        private readonly LockObject _locker = new LockObject();
        private volatile bool _canRead = true;
        private readonly RedisEventBus _redisEventBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _key;

        public InternalRedisEventHandler(string key, RedisEventBus redisEventBus, IServiceProvider serviceProvider)
        {
            _key = key;
            _redisEventBus = redisEventBus;
            _serviceProvider = serviceProvider;
        }

        internal void AddHandlers(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            _locker.Enter();
            _canRead = false;
            try
            {
                if (_handler.TryGetValue(eventRoute, out var list))
                {
                    list.AddRange(handlers);
                }
                else
                {
                    _handler.Add(eventRoute, handlers.ToList());
                }
            }
            finally
            {
                _canRead = true;
                _locker.Exit();
            }
        }

        internal async Task OnEvent(EventWrapper data)
        {
            if (data == null || data.Route == null || data.Event == null)
                return;

            if (!_canRead)
            {
                _locker.Enter();
                try
                {
                    await OnEventCoreAsync(data);
                }
                finally
                {
                    _locker.Exit();
                }
            }
            else
            {
                await OnEventCoreAsync(data);
            }
        }

        private async Task OnEventCoreAsync(EventWrapper data)
        {
            var eventRoute = data.Route;
            var eventTypeDict = new Dictionary<Type, IEvent>();

            IEvent @event;
            Type eventType;
            if (_handler.TryGetValue(eventRoute, out var list))
            {
                var redisEventContext = new RedisEventContext
                {
                    Key = _key,
                    EventBus = _redisEventBus,
                    ServiceProvider = _serviceProvider,
                    EventRoute = eventRoute,
                };
                foreach (var handle in list)
                {
                    try
                    {
                        eventType = handle.GetEventType();
                        if (!eventTypeDict.ContainsKey(eventType))
                        {
                            @event = data.Event.ToObject(eventType) as IEvent;
                            eventTypeDict.Add(eventType, @event);
                        }
                        else
                        {
                            @event = eventTypeDict[eventType];
                        }
                        await handle.OnEvent(@event, redisEventContext);
                    }
                    catch (Exception ex)
                    {
                        LogRecord.Error(nameof(InternalRedisEventHandler), $"eventRoute: {eventRoute}, exception: {ex.ToString()}");
                    }
                }
            }
        }
    }
}
