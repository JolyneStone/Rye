using Rye.EventBus;

using System.Diagnostics;
using System.Threading.Tasks;

namespace Rye.Test.EventBus
{
    /// <summary>
    /// 使用Redis或RabbitMQ时，需加上此特性
    /// </summary>
    [EventType(typeof(TestEvent))]
    public class TestEventHandler : Rye.EventBus.EventHandler<TestEvent>
    {
        public int Id { get; set; }
        protected override Task OnEvent(TestEvent @event)
        {
            Debug.WriteLine($"handler: {Id}, event: {@event.Id}");
            return Task.CompletedTask;
        }
    }
}
