using Rye.EventBus.Abstractions;

namespace Rye.EventBus.InMemory
{
    public interface IMemoryEventBus: IEventBus, IMemoryEventPublisher, IMemoryEventSubscriber
    {
    }
}
