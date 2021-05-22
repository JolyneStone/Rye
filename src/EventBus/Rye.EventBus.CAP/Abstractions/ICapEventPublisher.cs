using Rye.EventBus.Abstractions;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.EventBus.CAP
{
    public interface ICapEventPublisher: IEventPublisher
    {
        Task PublishAsync(string eventRoute, IEvent @event, Dictionary<string, string> header);

        Task PublishAsync<T>(string eventRoute, IEvent @event, string callbackName = null);
    }
}
