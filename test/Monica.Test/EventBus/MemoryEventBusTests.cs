using Microsoft.Extensions.DependencyInjection;

using Monica.EventBus.Abstractions;

using System;
using System.Diagnostics;
using System.Threading;

using Xunit;

namespace Monica.EventBus.Memory.Tests
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
            eventBus.AddHandlers<MemoryEventTest, MemoryEventHandlerTest>(
                new MemoryEventHandlerTest[] { new MemoryEventHandlerTest() { Id = 0 }, new MemoryEventHandlerTest() { Id = 1 } }
            );

            eventBus.Subscribe();
            for (var i = 0; i < 10; i++)
            {
                eventBus.Pushblish(new MemoryEventTest { Id = i });
                Thread.Sleep(200);
            }
            eventBus.Dispose();
            Assert.True(true);
        }

        public class MemoryEventTest : IEvent<MemoryEventTest>
        {
            public int Id { get; set; }
        }

        public class MemoryEventHandlerTest : MemoryEventHandler<MemoryEventTest>
        {
            public int Id { get; set; }
            public override void Handle(MemoryEventTest @event)
            {
                Debug.WriteLine($"{Id}");
            }
        }
    }
}