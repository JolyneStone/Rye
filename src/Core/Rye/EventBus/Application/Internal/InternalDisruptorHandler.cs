using Rye.EventBus.Abstractions;
using Rye.Logger;
using Rye.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.EventBus.Application.Internal
{
    internal class InternalDisruptorHandler : Disruptor.IEventHandler<EventWarpper>
    {
        private readonly Dictionary<string, List<IEventHandler>> _handler = new Dictionary<string, List<IEventHandler>>();
        private readonly LockObject _locker = new LockObject();
        private volatile bool _canRead = true;

        public void AddHandlers(string eventRoute, IEnumerable<IEventHandler> handlers)
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

        public void OnEvent(EventWarpper data, long sequence, bool endOfBatch)
        {
            if (data == null && data.EventRoute == null && data.Event == null)
                return;

            if (!_canRead)
            {
                _locker.Enter();
                try
                {
                    OnEventCore(data);
                }
                finally
                {
                    _locker.Exit();
                }
            }
            else
            {
                OnEventCore(data);
            }
        }

        private void OnEventCore(EventWarpper data)
        {
            if (_handler.TryGetValue(data.EventRoute, out var list))
            {
                foreach (var handle in list)
                {
                    try
                    {
                        handle.OnEvent(data.Event);
                    }
                    catch (Exception ex)
                    {
                        LogRecord.Error("ApplicationEventBus", $"eventRoute: {data.EventRoute}, exception: {ex.ToString()}");
                    }
                }
            }
        }
    }
}
