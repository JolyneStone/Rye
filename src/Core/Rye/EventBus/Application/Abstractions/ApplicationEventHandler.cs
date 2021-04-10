using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus.Application
{
    public abstract class ApplicationEventHandler<TEvent>: EventHandler<TEvent, ApplicationEventContext>
        where TEvent : IEvent
    {
    }
}
