using Polly;
using Polly.Retry;

using Rye.EventBus.Abstractions;
using Rye.Logger;

using System;

namespace Rye.EventBus.InMemory.Policy
{
    //public static class ApplicationEventBusRetryPolicy
    //{
        //public static Func<IEvent, RetryPolicy> ProducerRetryPolicy(int retryCount = 5)=>
        //    @event =>
        //        RetryPolicy.Handle<RyeEventBusException>()
        //        .Or<Disruptor.AlertException>()
        //        .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time, count, context) =>
        //        {
        //            LogRecord.Warn("ApplicationEventBus_Retry", $"无法推送事件, retryCount: {count}, time: {time.TotalSeconds:n1}, event: {@event.ToJsonString()}, exception: {ex.ToString()} ");
        //        });

        //public static Func<IEvent, AsyncRetryPolicy> AsyncProducerRetryPolicy(int retryCount = 5) =>
        //    @event =>
        //        RetryPolicy.Handle<RyeEventBusException>()
        //        .Or<Disruptor.AlertException>()
        //        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time, count, context) =>
        //        {
        //            LogRecord.Warn("ApplicationEventBus_Retry", $"无法推送事件, retryCount: {count}, time: {time.TotalSeconds:n1}, event: {@event.ToJsonString()}, exception: {ex.ToString()} ");
        //        });

        //public static Func<IEvent, RetryPolicy> ConsumerRetryPolicy(int retryCount = 5) =>
        //    @event =>
        //        RetryPolicy.Handle<Exception>()
        //        .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time, count, context) =>
        //        {
        //            LogRecord.Warn("ApplicationEventBus_Retry", $"无法消费事件, retryCount: {count}, time: {time.TotalSeconds:n1}, event: {@event.ToJsonString()}, exception: {ex.ToString()} ");
        //        });

        //public static Func<IEvent, AsyncRetryPolicy> ConsumerRetryPolicy(int retryCount = 5) =>
        //    @event =>
        //        RetryPolicy.Handle<Exception>()
        //        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time, count, context) =>
        //        {
        //            LogRecord.Warn("ApplicationEventBus_Retry", $"无法消费事件, retryCount: {count}, time: {time.TotalSeconds:n1}, event: {@event.ToJsonString()}, exception: {ex.ToString()} ");
        //        });
    //}
}
