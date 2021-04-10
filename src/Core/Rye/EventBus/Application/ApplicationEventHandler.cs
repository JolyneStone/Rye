using Rye.EventBus.Abstractions;

using System.Threading.Tasks;

namespace Rye.EventBus.Application
{
    public abstract class ApplicationEventHandler<TEvent> : IEventHandler
        where TEvent: IEvent
    {
        public void OnEvent(IEvent @event)
        {
            if (@event is TEvent e)
                OnEvent(e);
        }

        protected abstract void OnEvent(TEvent @event);
    }

    public abstract class ApplicationEventHandlerAsync<TEvent> : IEventHandler
        where TEvent : IEvent
    {
        public async void OnEvent(IEvent @event)
        {
            if (@event is TEvent e)
                await OnEventAsync(e);
        }

        protected abstract Task OnEventAsync(TEvent @event);
    }
}
