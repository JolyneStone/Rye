using Rye.EventBus.Abstractions;

namespace Rye.EventBus.InMemory.Internal
{
    internal class EventWrapper
    {
        public string Route { get; set; }
        public IEvent Event { get; set; }
        public int RetryCount { get; set; }
    }
}
