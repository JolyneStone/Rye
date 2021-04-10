using Rye.EventBus.Abstractions;
using Rye.Logger;
using Rye.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Rye.EventBus.Application.Internal
{
    internal class InternalDisruptorHandler : Disruptor.IEventHandler<EventWrapper>
    {
        private readonly Dictionary<string, List<IEventHandler>> _handler = new Dictionary<string, List<IEventHandler>>();
        private readonly LockObject _locker = new LockObject();
        private volatile bool _canRead = true;
        private readonly ApplicationEventBus _applicationEventBus;
        private readonly IServiceProvider _serviceProvider;

        public InternalDisruptorHandler(
            ApplicationEventBus applicationEventBus,
            IServiceProvider serviceProvider)
        {
            _applicationEventBus = applicationEventBus;
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

        public async void OnEvent(EventWrapper data, long sequence, bool endOfBatch)
        {
            if (data == null && data.EventRoute == null && data.Event == null)
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
            if (_handler.TryGetValue(data.EventRoute, out var list))
            {
                var applicationEventContext = new ApplicationEventContext()
                {
                    EventBus = _applicationEventBus,
                    ServiceProvider = _serviceProvider,
                    EventRoute = data.EventRoute
                };
                foreach (var handle in list)
                {
                    try
                    {
                        await handle.OnEvent(data.Event, applicationEventContext);
                    }
                    catch (Exception ex)
                    {
                        LogRecord.Error(nameof(InternalDisruptorHandler), $"eventRoute: {data.EventRoute}, event: {data.Event.ToJsonString()} exception: {ex.ToString()}");
                    }
                }
            }
        }
    }
}
