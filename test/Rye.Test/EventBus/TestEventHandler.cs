using Rye.EventBus;
using Rye.EventBus.Lightweight;
using Rye.EventBus.Redis;

using System.Diagnostics;
using System.Threading.Tasks;

namespace Rye.Test.EventBus
{
    public class TestApplicationEventHandler : Rye.EventBus.Lightweight.LightweightEventHandler<TestEvent>
    {
        public int Id { get; set; }
        protected override Task OnEvent(TestEvent @event, LightweightEventContext eventContext)
        {
            Debug.WriteLine($"handler: {Id}, event: {@event.Id}");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 使用Redis或RabbitMQ时，需加上此特性
    /// </summary>
    [EventType(typeof(TestEvent))]
    public class TestRedisEventHandler : Rye.EventBus.Redis.RedisEventHandler<TestEvent>
    {
        public int Id { get; set; }
        protected override Task OnEvent(TestEvent @event, RedisEventContext eventContext)
        {
            Debug.WriteLine($"handler: {Id}, event: {@event.Id}");
            return Task.CompletedTask;
        }
    }
}
