using Rye.EventBus.Abstractions;

namespace Rye.EventBus.Lightweight
{
    public interface ILightweightEventBus: IEventBus, ILightweightEventPublisher, ILightweightEventSubscriber
    {
    }
}
