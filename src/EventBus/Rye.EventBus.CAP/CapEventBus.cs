using DotNetCore.CAP;

using Rye.EventBus.Abstractions;
using Rye.Util;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.EventBus.CAP
{
    public class CapEventBus : ICapEventBus
    {
        private readonly ICapPublisher _capPublisher;
        public CapEventBus(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task PublishAsync(string eventRoute, IEvent @event)
        {
            return PublishAsync(eventRoute, @event, new Dictionary<string, string>
            {
                { "retry-count", "0" }
            });
        }

        public Task PublishAsync(string eventRoute, IEvent @event, Dictionary<string, string> header)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            return _capPublisher.PublishAsync(eventRoute, @event, header);
        }

        public Task PublishAsync<T>(string eventRoute, IEvent @event, string callbackName = null)
        {
            Check.NotNull(eventRoute, nameof(eventRoute));
            Check.NotNull(@event, nameof(@event));

            if (@event.EventId == 0)
            {
                @event.EventId = IdGenerator.Instance.NextId();
            }

            return _capPublisher.PublishAsync(eventRoute, @event, callbackName);
        }


        public Task RetryEvent(IEvent @event, EventContext context)
        {
            Check.NotNull(@event, nameof(@event));
            Check.NotNull(context, nameof(context));
            var header = (context as CapEventPublishContext)?.Header;
            if (header != null && header.TryGetValue("retry-count", out string retryCount))
            {
                var dict = header.ToDictionary(d => d.Key, d => d.Value);
                dict["retry-count"] = (retryCount.ParseByInt() + 1).ToString();
                header = new CapHeader(dict);
            }
            return _capPublisher.PublishAsync(context.RouteKey, @event, header);
        }

        public void Subscribe(string eventRoute, IEnumerable<IEventHandler> handlers)
        {
            //Check.NotNull(eventRoute, nameof(eventRoute));
            //Check.NotNull(handlers, nameof(handlers));

            //InternalCapEventHandler.AddHandlers(eventRoute, handlers);
            return;
        }

        public void Dispose()
        {
        }
    }
}
