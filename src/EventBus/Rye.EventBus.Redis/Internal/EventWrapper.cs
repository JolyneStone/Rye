using Rye.EventBus.Abstractions;

namespace Rye.EventBus.Redis.Internal
{
    internal class EventWrapper
    {
        public string Route { get; set; }
        public string Event { get; set; }
    }
}
