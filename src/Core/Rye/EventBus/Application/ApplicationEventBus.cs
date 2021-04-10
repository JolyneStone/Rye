using Rye.EventBus.Abstractions;
using Rye.EventBus.Application.Internal;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rye.EventBus.Application
{
    public class ApplicationEventBus : IEventBus
    {
        private readonly int _ringBufferSize;
        private Disruptor.Dsl.Disruptor<EventWarpper> _disruptor;
        private Disruptor.RingBuffer<EventWarpper> _ringBuffer;
        private InternalDisruptorHandler _handler;
        public ApplicationEventBus() : this(1024)
        {
        }

        public ApplicationEventBus(int bufferSize)
        {
            _ringBufferSize = bufferSize;
            _handler = new InternalDisruptorHandler();
            _disruptor = new Disruptor.Dsl.Disruptor<EventWarpper>(() => new EventWarpper(), _ringBufferSize, TaskScheduler.Default);
            _disruptor.HandleEventsWith(_handler);
            _ringBuffer = _disruptor.Start();
        }

        public Task PublishAsync(string eventName, IEvent @event)
        {
            Publish(eventName, @event);
            return Task.CompletedTask;
        }

        public void Publish(string eventRoute, IEvent @event)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            long sequence = _ringBuffer.Next();
            var wapper = _ringBuffer[sequence];
            wapper.EventRoute = eventRoute;
            wapper.Event = @event;
            _ringBuffer.Publish(sequence);
        }

        public void Subscribe(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(handlers, nameof(handlers));

            _handler.AddHandlers(eventRoute, handlers);
        }

        #region IDisposable Support
        private bool disposedValue = false;
        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _disruptor?.Shutdown();
                    _ringBuffer = null;
                    if (_handler != null)
                    {
                        _handler = null;
                    }

                    GC.SuppressFinalize(this);
                }

                disposedValue = true;
            }
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
