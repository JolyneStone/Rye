using Rye.EventBus;
using Rye.EventBus.InMemory;
using Rye.EventBus.Redis;

using System.Diagnostics;
using System.Threading.Tasks;

namespace Rye.Test.EventBus
{
    public class TestApplicationEventHandler : Rye.EventBus.InMemory.InMemoryEventHandler<TestEvent>
    {
        public int Id { get; set; }
        protected override Task OnEvent(TestEvent @event, InMemoryEventContext eventContext)
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
