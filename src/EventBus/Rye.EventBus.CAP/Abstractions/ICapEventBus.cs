using Rye.EventBus.Abstractions;

namespace Rye.EventBus.CAP
{
    public interface ICapEventBus: IEventBus, ICapEventPublisher, ICapEventSubscriber
    {
    }
}
