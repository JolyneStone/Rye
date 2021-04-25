using Polly.Retry;

using Rye.EventBus.Abstractions;

using System;
using System.Threading.Tasks;

namespace Rye.EventBus.Application.Options
{
    public class ApplicationEventBusOptions
    {
        /// <summary>
        /// 缓存区大小
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        public Func<IEvent, ApplicationEventContext, Task> OnProducing { get; set; }

        public Func<IEvent, ApplicationEventContext, Task> OnProduced { get; set; }

        public Func<IEvent, ApplicationEventErrorContext, Task> OnProductError { get; set; }

        public Func<IEvent, ApplicationEventContext, Task> OnConsuming { get; set; }

        public Func<IEvent, ApplicationEventContext, Task> OnConsumed { get; set; }

        public Func<IEvent, ApplicationEventErrorContext, Task> OnConsumeError { get; set; }

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
