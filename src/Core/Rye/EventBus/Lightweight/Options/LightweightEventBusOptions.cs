using Polly.Retry;

using Rye.EventBus.Abstractions;

using System;
using System.Threading.Tasks;

namespace Rye.EventBus.Lightweight.Options
{
    public class LightweightEventBusOptions
    {
        /// <summary>
        /// 缓存区大小
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        public Func<IEvent, LightweightEventContext, Task> OnProducing { get; set; }

        public Func<IEvent, LightweightEventContext, Task> OnProduced { get; set; }

        public Func<IEvent, LightweightEventErrorContext, Task> OnProductError { get; set; }

        public Func<IEvent, LightweightEventContext, Task> OnConsuming { get; set; }

        public Func<IEvent, LightweightEventContext, Task> OnConsumed { get; set; }

        public Func<IEvent, LightweightEventErrorContext, Task> OnConsumeError { get; set; }

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
