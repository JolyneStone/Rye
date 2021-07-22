using Nito.AsyncEx;

using RabbitMQ.Client.Events;

using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ.Internal
{
    internal class InternalRabbitMQEventHandler
    {
        private readonly Dictionary<string, List<IEventHandler>> _handler = new Dictionary<string, List<IEventHandler>>();
        private readonly AsyncLock _locker = new AsyncLock();
        private volatile bool _canRead = true;

        internal void AddHandlers(string route, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(handlers, nameof(handlers));

            using (_locker.Lock())
            {
                _canRead = false;
                try
                {
                    if (_handler.TryGetValue(route, out var list))
                    {
                        list.AddRange(handlers);
                    }
                    else
                    {
                        _handler.Add(route, handlers.ToList());
                    }
                }
                finally
                {
                    _canRead = true;
                }
            }
        }

        internal async Task OnEvent(EventWrapper data, BasicDeliverEventArgs eventArgs)
        {
            if (data == null || data.Exchange == null || data.Queue == null || data.RouteKey == null || data.Event == null)
                return;

            if (!_canRead)
            {
                using (await _locker.LockAsync())
                {
                    await OnEventCoreAsync(data, eventArgs);
                }
            }
            else
            {
                await OnEventCoreAsync(data, eventArgs);
            }
        }

        private async Task OnEventCoreAsync(EventWrapper wrapper, BasicDeliverEventArgs eventArgs)
        {
            var eventRoute = wrapper.Key;
            if (_handler.TryGetValue(eventRoute, out var list))
            {
                await OnConsumeEvent?.Invoke(eventArgs, wrapper, list);
            }
        }

        public event Func<BasicDeliverEventArgs, EventWrapper, List<IEventHandler>,  Task> OnConsumeEvent;
    }
}
