using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.EventBus.Abstractions;
using Rye.EventBus.Lightweight.Internal;
using Rye.EventBus.Lightweight.Options;
using Rye.Util;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.EventBus.Lightweight
{
    public class LightweightEventBus : ILightweightEventBus
    {
        private readonly Disruptor.Dsl.Disruptor<EventWrapper> _disruptor;
        private readonly Disruptor.RingBuffer<EventWrapper> _ringBuffer;
        private readonly InternalDisruptorHandler _handler;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public event Func<IEvent, LightweightEventContext, Task> OnProducing;

        public event Func<IEvent, LightweightEventContext, Task> OnProduced;

        public event Func<IEvent, LightweightEventErrorContext, Task> OnProductError;

        public event Func<IEvent, LightweightEventContext, Task> OnConsuming;

        public event Func<IEvent, LightweightEventContext, Task> OnConsumed;

        public event Func<IEvent, LightweightEventErrorContext, Task> OnConsumeError;

        public LightweightEventBus(IOptions<LightweightEventBusOptions> options, IServiceScopeFactory scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
            _handler = new InternalDisruptorHandler();
            _handler.OnConsumeEvent += OnConsumeEvent;

            var busOptions = options.Value;

            if (busOptions.OnProducing != null)
                OnProducing = busOptions.OnProducing;
            if (busOptions.OnProduced != null)
                OnProduced = busOptions.OnProduced;
            if (busOptions.OnProductError != null)
                OnProductError = busOptions.OnProductError;
            if (busOptions.OnConsuming != null)
                OnConsuming = busOptions.OnConsuming;
            if (busOptions.OnConsumed != null)
                OnConsumed = busOptions.OnConsumed;
            if (busOptions.OnConsumeError != null)
                OnConsumeError = busOptions.OnConsumeError;

            _disruptor = new Disruptor.Dsl.Disruptor<EventWrapper>(
                eventFactory: () => new EventWrapper(),
                ringBufferSize: busOptions.BufferSize,
                taskScheduler: TaskScheduler.Default,
                producerType: Disruptor.Dsl.ProducerType.Single,
                waitStrategy: new Disruptor.YieldingWaitStrategy());
            _disruptor.HandleEventsWith(_handler);
            _ringBuffer = _disruptor.Start();
        }

        private async Task OnConsumeEvent(EventWrapper wrapper, List<IEventHandler> handlers)
        {
            var @event = wrapper.Event;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var context = new LightweightEventContext()
                    {
                        EventBus = this,
                        ServiceProvider = scope.ServiceProvider,
                        RouteKey = wrapper.Route,
                        RetryCount = wrapper.RetryCount
                    };

                    if (OnConsuming != null)
                        await OnConsuming(@event, context);

                    foreach (var handle in handlers)
                    {
                        await handle.OnEvent(wrapper.Event, context);
                    }

                    if (OnConsumed != null)
                        await OnConsumed(@event, context);
                }
                catch (Exception ex)
                {
                    if (OnConsumeError == null)
                        throw;

                    var context = new LightweightEventErrorContext
                    {
                        EventBus = this,
                        ServiceProvider = scope.ServiceProvider,
                        RouteKey = wrapper.Route,
                        RetryCount = wrapper.RetryCount,
                        Exception = ex
                    };
                    await OnConsumeError(@event, context);
                }
            }
        }

        public async Task PublishAsync(string eventRoute, IEvent @event)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            await PublishAsync(eventRoute, @event, 0);
        }

        private async Task PublishAsync(string eventRoute, IEvent @event, int retryCount = 0)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var context = new LightweightEventContext
                    {
                        ServiceProvider = scope.ServiceProvider,
                        EventBus = this,
                        RouteKey = eventRoute,
                        RetryCount = retryCount
                    };

                    if (OnProducing != null)
                        await OnProducing(@event, context);

                    long sequence = _ringBuffer.Next();
                    var wapper = _ringBuffer[sequence];
                    wapper.Route = eventRoute;
                    wapper.Event = @event;
                    wapper.RetryCount = retryCount;
                    _ringBuffer.Publish(sequence);

                    if (OnProduced != null)
                        await OnProduced(@event, context);
                }
                catch (Exception ex)
                {
                    if (OnProductError == null)
                        throw;

                    var context = new LightweightEventErrorContext
                    {
                        ServiceProvider = scope.ServiceProvider,
                        EventBus = this,
                        RouteKey = eventRoute,
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

        public Task RetryEvent(IEvent @event, EventContext context)
        {
            return PublishAsync(context.RouteKey, @event, context.RetryCount + 1);
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
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
