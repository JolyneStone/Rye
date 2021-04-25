using System;
using System.Threading.Tasks;

namespace Rye.EventBus.Abstractions
{
    public interface IEventPublisher : IDisposable
    {
        Task PublishAsync(string eventRoute, IEvent @event);
    }
}
