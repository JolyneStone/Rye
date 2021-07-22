using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus
{
    public abstract class EventHandler : IEventHandler
    {
        public abstract Task OnEvent(IEvent @event, EventContext eventContext);
    }

    public abstract class EventHandler<TEvent, TContext> : IEventHandler
        where TEvent: IEvent
        where TContext: EventContext
    {
        public async Task OnEvent(IEvent @event, EventContext eventContext)
        {
            if (@event != null && @event is TEvent e)
                await OnEvent(e, eventContext as TContext);
            else
                await OnEvent(default(TEvent), eventContext as TContext);
        }

        protected abstract Task OnEvent(TEvent @event, TContext eventContext);
    }
}
