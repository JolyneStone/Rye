namespace Rye.EventBus.Abstractions
{
    public interface IEventHandler
    {
        void OnEvent(IEvent @event);
    }
}
