using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus
{
    public abstract class EventHandler : IEventHandler
    {
        public abstract Task OnEvent(IEvent @event);
    }

    public abstract class EventHandler<TEvent> : IEventHandler
        where TEvent: IEvent
    {
        public async Task OnEvent(IEvent @event)
        {
            if (@event != null && @event is TEvent e)
                await OnEvent(e);
        }

        protected abstract Task OnEvent(TEvent @event);
    }
}
