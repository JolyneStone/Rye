using Monica.EventBus.Abstractions;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Monica.EventBus.Memory
{
    public class MemoryEventBus : IEventBus
    {
        private static readonly int RingBufferSize = 1024;
        private static ConcurrentDictionary<string, Disruptor.RingBuffer> _ringBuffers = new ConcurrentDictionary<string, Disruptor.RingBuffer>();
        private static Dictionary<string, object> _pools = new Dictionary<string, object>();
        private static Dictionary<string, List<object>> _handlers = new Dictionary<string, List<object>>();

        public Task PublishAsync<TEvent>(string eventName, TEvent @event)
            where TEvent : class, IEvent<TEvent>, new()
        {
            Pushblish(eventName, @event);
            return Task.CompletedTask;
        }

        public void Pushblish<TEvent>(string eventName, TEvent @event)
            where TEvent : class, IEvent<TEvent>, new()
        {
            if (eventName is null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!_ringBuffers.TryGetValue(eventName, out var value))
            {
                throw new MonicaEventBusException($"事件{eventName}未注册到EventBus中");
            }

            var ringBuffer = value as Disruptor.RingBuffer<TEvent>;
            long sequence = ringBuffer.Next();
            try
            {
                var entry = ringBuffer[sequence];
                @event.Clone(entry);
            }
            finally
            {
                ringBuffer.Publish(sequence);
            }
        }

        public void Subscribe()
        {
            var method = this.GetType().GetMethod(nameof(SubscribeCore), BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var item in _handlers)
            {
                var eventName = item.Key;
                var handlers = item.Value;
                if (handlers.Count <= 0)
                {
                    throw new ArgumentException($"事件{eventName}未注册正确的事件处理器");
                }

                method.MakeGenericMethod(handlers[0].GetType()
                                                      .GetInterfaces()
                                                      .First(d => d.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                                                      .GetGenericArguments()[0]
                                        ).Invoke(this, new object[] { eventName, handlers });
            }

            _handlers.Clear();
        }

        private void SubscribeCore<TEvent>(string eventName, List<object> handlers) where TEvent : class, IEvent<TEvent>, new()
        {
            var disruptorHandlers = handlers.Select(d => d as Disruptor.IEventHandler<TEvent>).ToArray();
            if (!_ringBuffers.TryGetValue(eventName, out var value))
            {
                var ringBuffer = value as Disruptor.RingBuffer<TEvent>;
                var disruptor = new Disruptor.Dsl.Disruptor<TEvent>(() => new TEvent(), RingBufferSize, TaskScheduler.Default);

                // 多生产者多消费者，但不重复消费消息模式
                //if (disruptorHandlers.Length == 1)
                //{
                //    ringBuffer = Disruptor.RingBuffer<TEvent>.CreateSingleProducer(() => new TEvent(), RingBufferSize, new Disruptor.YieldingWaitStrategy());
                //}
                //else
                //{
                //    ringBuffer = Disruptor.RingBuffer<TEvent>.CreateMultiProducer(() => new TEvent(), RingBufferSize, new Disruptor.YieldingWaitStrategy());
                //}

                //var workerPool = new Disruptor.WorkerPool<TEvent>(ringBuffer, ringBuffer.NewBarrier(),
                //    new Disruptor.FatalExceptionHandler(), disruptorHandlers);
                //ringBuffer.AddGatingSequences(workerPool.GetWorkerSequences());

                //workerPool.Start(new Disruptor.Dsl.BasicExecutor(TaskScheduler.Default));
                //_ringBuffers.TryAdd(eventType, ringBuffer);
                //_pools.TryAdd(eventType, workerPool);

                disruptor.HandleEventsWith(disruptorHandlers);
                ringBuffer = disruptor.Start();
                _ringBuffers.TryAdd(eventName, ringBuffer);
                _pools.TryAdd(eventName, disruptor);
            }
        }

        public void AddHandlers<TEvent, THandler>(string eventName, IEnumerable<THandler> handlers)
            where TEvent : class, IEvent<TEvent>, new()
            where THandler : IEventHandler<TEvent>
        {
            if (eventName is null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (handlers is null && handlers.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(handlers));
            }

            var disruptorHandlers = handlers.Where(d => d is Disruptor.IEventHandler<TEvent>).ToArray();

            if (!_handlers.TryGetValue(eventName, out var list) || list == null)
            {
                if (disruptorHandlers != null && disruptorHandlers.Length > 0)
                {
                    _handlers.TryAdd(eventName, disruptorHandlers.Select(d => (object)d).ToList());
                }
            }
            else
            {
                foreach (var item in disruptorHandlers)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }

                _handlers[eventName] = list;
            }
        }

        private void HaltCore<TEvent>(object obj) where TEvent : class, IEvent<TEvent>, new()
        {
            if (obj != null)
            {
                (obj as Disruptor.Dsl.Disruptor<TEvent>).Shutdown();
            }
        }


        #region IDisposable Support
        private bool disposedValue = false;
        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_pools != null)
                    {
                        var method = this.GetType().GetMethod(nameof(HaltCore), BindingFlags.NonPublic | BindingFlags.Instance);
                        foreach (var item in _pools)
                        {
                            method.MakeGenericMethod(item.Value.GetType().GetGenericArguments()[0]).Invoke(this, new object[] { item.Value });
                        }
                        _pools.Clear();
                        _pools = null;
                    }
                    if (_ringBuffers != null)
                    {
                        _ringBuffers.Clear();
                        _ringBuffers = null;
                    }
                    if (_handlers != null)
                    {
                        _handlers.Clear();
                        _handlers = null;
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
