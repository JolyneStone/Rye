using System.Threading.Tasks;

namespace Rye.EventBus.Abstractions
{
    public interface ISubscribeEventHandler
    {
    }

    public interface IEventHandler: ISubscribeEventHandler
    {
        Task OnEvent(IEvent @event, EventContext eventContext);
    }
}
