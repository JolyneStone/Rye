using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;
using Rye.Test.EventBus;

using System.Threading.Tasks;

using Xunit;

namespace Rye.EventBus.Application.Tests
{
    public class ApplicationEventBusTests
    {
        [Fact()]
        public async Task PushblishTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationEventBusModule()
                .ConfigureModule();
            var services = serviceCollection.BuildServiceProvider();

            var eventBus = services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TestEvent>(
                new TestEventHandler[] { new TestEventHandler() { Id = 0 }, new TestEventHandler() { Id = 1 } }
            );

            for (var i = 0; i < 10; i++)
            {
                eventBus.Publish(new TestEvent { Id = i });
                await Task.Delay(200);
            }

            await Task.Delay(20000);
            eventBus.Dispose();
            Assert.True(true);
        }
    }
}