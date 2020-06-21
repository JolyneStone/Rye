using Raven.EventBus.Abstractions;

namespace Raven.EventBus.Memory
{
    public abstract class MemoryEventHandler<TEvent> : IEventHandler<TEvent>, Disruptor.IEventHandler<TEvent>
        where TEvent : class, IEvent<TEvent>, new()
    {
        public abstract void Handle(TEvent @event);

        public void OnEvent(TEvent @event, long sequence, bool endOfBatch)
        {
            Handle(@event);
        }
    }
}
