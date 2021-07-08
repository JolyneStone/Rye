using Nito.AsyncEx;

using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.EventBus.Lightweight.Internal
{
    internal class InternalDisruptorHandler : Disruptor.IEventHandler<EventWrapper>
    {
        private readonly Dictionary<string, List<IEventHandler>> _handler = new Dictionary<string, List<IEventHandler>>();
        private readonly AsyncLock _locker = new AsyncLock();
        private volatile bool _canRead = true;

        internal void AddHandlers(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            using (_locker.Lock())
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

        public async void OnEvent(EventWrapper data, long sequence, bool endOfBatch)
        {
            if (data == null && data.Route == null && data.Event == null)
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
            if (_handler.TryGetValue(wrapper.Route, out var list))
            {
                //var applicationEventContext = new ApplicationEventContext()
                //{
                //    EventBus = _applicationEventBus,
                //    ServiceProvider = _serviceProvider,
                //    EventRoute = data.EventRoute
                //};
                //foreach (var handle in list)
                //{
                //    try
                //    {
                //        await handle.OnEvent(data.Event, applicationEventContext);
                //    }
                //    catch (Exception ex)
                //    {
                //        LogRecord.Error(nameof(InternalDisruptorHandler), $"eventRoute: {data.EventRoute}, event: {data.Event.ToJsonString()} exception: {ex.ToString()}");
                //    }
                //}
                if (OnConsumeEvent != null)
                    await OnConsumeEvent(wrapper, list);
            }
        }

        public event Func<EventWrapper, List<IEventHandler>, Task> OnConsumeEvent;
    }
}
