using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus.Lightweight
{
    public abstract class LightweightEventHandler<TEvent>: EventHandler<TEvent, LightweightEventContext>
        where TEvent : IEvent
    {
    }
}
