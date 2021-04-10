using Rye.EventBus.Abstractions;

namespace Rye.EventBus.Application.Internal
{
    internal class EventWrapper
    {
        public string EventRoute { get; set; }
        public IEvent Event { get; set; }
    }
}
