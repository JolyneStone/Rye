using System;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.EventBus.Abstractions
{
    public interface IEventPublisher : IDisposable
    {
        void Pushblish<TEvent>(TEvent @event) where TEvent : class, IEvent<TEvent>, new();
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent<TEvent>, new();
    }
}
