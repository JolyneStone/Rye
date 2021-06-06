using Polly.Retry;

using RabbitMQ.Client;

using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.EventBus.RabbitMQ.Options
{
    public class RabbitMQEventBusOptions
    {
        /// <summary>
        /// 连接重试次数
        /// </summary>
        public int RetryCount { get; set; } = 5;

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// RabbitMQ 连接工厂
        /// </summary>
        public IConnectionFactory ConnectionFactory { get; set; }

        public Func<IEvent, RabbitMQEventPublishContext, Task> OnProducing { get; set; }

        public Func<IEvent, RabbitMQEventPublishContext, Task> OnProduced { get; set; }

        public Func<IEvent, RabbitMQEventPublishErrorContext, Task> OnProductError { get; set; }

        public Func<IEvent, RabbitMQEventSubscribeContext, Task> OnConsuming { get; set; }

        public Func<IEvent, RabbitMQEventSubscribeContext, Task> OnConsumed { get; set; }

        public Func<IEvent, RabbitMQEventSubscribeErrorContext, Task> OnConsumeError { get; set; }
    }
}
