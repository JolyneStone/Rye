using Rye.EventBus.Abstractions;

namespace Rye.EventBus.Lightweight
{
    public interface IMemoryEventBus: IEventBus, IMemoryEventPublisher, IMemoryEventSubscriber
    {
    }
}
