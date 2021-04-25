using Nito.AsyncEx;

using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.EventBus.Redis.Internal
{
    internal class InternalRedisEventHandler
    {
        private readonly Dictionary<string, List<IEventHandler>> _handler = new Dictionary<string, List<IEventHandler>>();
        private readonly AsyncLock _locker = new AsyncLock();
        private volatile bool _canRead = true;

        internal void AddHandlers(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            using(_locker.Lock())
            {
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
                }
            }
        }

        internal async Task OnEvent(EventWrapper data)
        {
            if (data == null || data.Route == null || data.Event == null)
                return;

            if (!_canRead)
            {
                using (await _locker.LockAsync())
                {
                    await OnEventCoreAsync(data);
                }
            }
            else
            {
                await OnEventCoreAsync(data);
            }
        }

        private async Task OnEventCoreAsync(EventWrapper wrapper)
        {
            var eventRoute = wrapper.Route;

            if (_handler.TryGetValue(eventRoute, out var list))
            {
                //var redisEventContext = new RedisEventContext
                //{
                //    Key = _key,
                //    EventBus = _redisEventBus,
                //    ServiceProvider = _serviceProvider,
                //    EventRoute = eventRoute,
                //};
                //foreach (var handle in list)
                //{
                //    try
                //    {
                //        eventType = handle.GetEventType();
                //        if (!eventTypeDict.ContainsKey(eventType))
                //        {
                //            @event = data.Event.ToObject(eventType) as IEvent;
                //            eventTypeDict.Add(eventType, @event);
                //        }
                //        else
                //        {
                //            @event = eventTypeDict[eventType];
                //        }
                //        await handle.OnEvent(@event, redisEventContext);
                //    }
                //    catch (Exception ex)
                //    {
                //        LogRecord.Error(nameof(InternalRedisEventHandler), $"eventRoute: {eventRoute}, exception: {ex.ToString()}");
                //    }
                //}
                await OnConsumeEvent?.Invoke(wrapper, list);
            }
        }

        public event Func<EventWrapper, List<IEventHandler>, Task> OnConsumeEvent;
    }
}
