using System.Threading;
using System.Threading.Tasks;

namespace Monica.EventBus.Abstractions
{
    public interface IEventHandler<in T> where T : class, IEvent<T>, new()
    {
        void Handle(T @event);
    }

    public interface IEventHandlerAsync<in T>: IEventHandler<T> where T : class, IEvent<T>, new()
    {
        Task HandleAsync(T @event, CancellationToken cancellationToken = default);
    }
}
