using Microsoft.Extensions.DependencyInjection;

using Rye.EventBus.Abstractions;
using Rye.Test;
using Rye.Test.EventBus;

using System.Threading.Tasks;

using Xunit;

namespace Rye.EventBus.InMemory.Tests
{
    public class ApplicationEventBusTests
    {
        [Fact()]
        public async Task PushblishTest()
        {
            var services = TestSetup.ConfigService(serviceCollection =>
            {
                serviceCollection
                    .AddInMemoryEventBusModule()
                    .ConfigureModule();
            });

            var eventBus = services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TestEvent>(
                new TestApplicationEventHandler[] { new TestApplicationEventHandler() { Id = 0 }, new TestApplicationEventHandler() { Id = 1 } }
            );

            for (var i = 0; i < 10; i++)
            {
                await eventBus.PublishAsync(new TestEvent { Id = i });
                await Task.Delay(200);
            }

            await Task.Delay(20000);
            eventBus.Dispose();
            Assert.True(true);
        }
    }
}