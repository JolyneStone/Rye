using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Rye.EventBus.Abstractions;

using System;

namespace Rye.EventBus.RabbitMQ
{
    public class RabbitMQEventPublishErrorContext : EventErrorContext
    {
        public string Exchange { get; init; }
        public string Queue { get; init; }
        public IBasicProperties BasicProperties { get; set; }
    }

    public class RabbitMQEventSubscribeErrorContext : EventErrorContext
    {
        private readonly IChannel _channel;
        public ReadOnlyMemory<byte> Body { get; init; }
        public string ConsumerTag { get; init; }
        public ulong DeliveryTag { get; init; }
        public bool Redelivered { get; init; }
        public bool Ack { get; set; }
        public string Exchange { get; init; }
        public string Queue { get; init; }
        public IReadOnlyBasicProperties BasicProperties { get; set; }

        public RabbitMQEventSubscribeErrorContext(IChannel channel, BasicDeliverEventArgs eventArgs)
        {
            BasicProperties = eventArgs.BasicProperties;
            Body = eventArgs.Body;
            ConsumerTag = eventArgs.ConsumerTag;
            DeliveryTag = eventArgs.DeliveryTag;
            Redelivered = eventArgs.Redelivered;
            RouteKey = eventArgs.RoutingKey;
            _channel = channel;
        }
    }
}
