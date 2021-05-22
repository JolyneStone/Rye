using Polly.Retry;

using Rye.EventBus.Abstractions;

using System;
using System.Threading.Tasks;

namespace Rye.EventBus.InMemory.Options
{
    public class InMemoryEventBusOptions
    {
        /// <summary>
        /// 缓存区大小
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        public Func<IEvent, InMemoryEventContext, Task> OnProducing { get; set; }

        public Func<IEvent, InMemoryEventContext, Task> OnProduced { get; set; }

        public Func<IEvent, InMemoryEventErrorContext, Task> OnProductError { get; set; }

        public Func<IEvent, InMemoryEventContext, Task> OnConsuming { get; set; }

        public Func<IEvent, InMemoryEventContext, Task> OnConsumed { get; set; }

        public Func<IEvent, InMemoryEventErrorContext, Task> OnConsumeError { get; set; }

        ///// <summary>
        ///// 异步发布事件的重试策略
        ///// </summary>
        //public Func<IEvent, AsyncRetryPolicy> AsyncProducerRetryPolicy { get; set; }

        /// <summary>
        /// 同步发布事件的重试策略
        /// </summary>
        //public Func<IEvent, RetryPolicy> ProducerRetryPolicy { get; set; }

        ///// <summary>
        ///// 异步消费事件的重试策略
        ///// </summary>
        //public Func<IEvent, AsyncRetryPolicy> ConsumerRetryPolicy { get; set; }

        ///// <summary>
        ///// 同步消费事件的重试策略
        ///// </summary>
        //public Func<IEvent, RetryPolicy> ConsumerRetryPolicy { get; set; }
    }
}
