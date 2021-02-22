using System;
using System.Threading.Tasks;

namespace Rye.EventBus.Abstractions
{
    public interface IEventPublisher : IDisposable
    {
        void Pushblish<TEvent>(string eventName, TEvent @event) where TEvent : class, IEvent<TEvent>, new();
        Task PublishAsync<TEvent>(string eventName, TEvent @event)
            where TEvent : class, IEvent<TEvent>, new();
    }
}
