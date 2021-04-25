namespace Rye.EventBus.RabbitMQ.Internal
{
    internal class EventWrapper
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string RouteKey { get; set; }
        public string Event { get; set; }
        public int RetryCount { get; set; }
    }
}
