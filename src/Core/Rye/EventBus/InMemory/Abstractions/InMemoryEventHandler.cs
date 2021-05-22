using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus.InMemory
{
    public abstract class InMemoryEventHandler<TEvent>: EventHandler<TEvent, InMemoryEventContext>
        where TEvent : IEvent
    {
    }
}
