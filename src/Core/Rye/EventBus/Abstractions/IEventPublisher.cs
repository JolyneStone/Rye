using System;
using System.Threading.Tasks;

namespace Rye.EventBus.Abstractions
{
    public interface IEventPublisher : IDisposable
    {
        void Publish(string eventRoute, IEvent @event);
        Task PublishAsync(string eventRoute, IEvent @event);
    }
}
