using System.Threading.Tasks;

namespace Rye.EventBus.Abstractions
{
    public interface IEventHandler
    {
        Task OnEvent(IEvent @event);
    }
}
