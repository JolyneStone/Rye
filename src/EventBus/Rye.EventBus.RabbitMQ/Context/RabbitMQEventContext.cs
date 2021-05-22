using RabbitMQ.Client;

using Rye.EventBus.Abstractions;

namespace Rye.EventBus.RabbitMQ
{
    public class RabbitMQEventPublishContext : EventContext
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public IBasicProperties BasicProperties { get; set; }

    }

    public class RabbitMQEventSubscribeContext: EventContext
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public IBasicProperties BasicProperties { get; set; }
        public bool Ack { get; set; }
    }
}
