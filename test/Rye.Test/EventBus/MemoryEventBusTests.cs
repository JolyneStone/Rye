using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;

using System.Diagnostics;
using System.Threading;

using Xunit;

namespace Rye.EventBus.Application.Tests
{
    public class MemoryEventBusTests
    {
        [Fact()]
        public void PushblishTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryEventBus();
            var services = serviceCollection.BuildServiceProvider();

            var eventBus = services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<MemoryEventTest>(
                new MemoryEventHandlerTest[] { new MemoryEventHandlerTest() { Id = 0 }, new MemoryEventHandlerTest() { Id = 1 } }
            );

            for (var i = 0; i < 10; i++)
            {
                eventBus.Publish(new MemoryEventTest { Id = i });
                Thread.Sleep(200);
            }
            eventBus.Dispose();
            Assert.True(true);
        }

        public class MemoryEventTest : IEvent
        {
            public int Id { get; set; }
        }

        public class MemoryEventHandlerTest : ApplicationEventHandler<MemoryEventTest>
        {
            public int Id { get; set; }
            protected override void OnEvent(MemoryEventTest @event)
            {
                Debug.WriteLine($"{Id}, event: {@event.Id}");
            }
        }
    }
}