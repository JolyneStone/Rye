using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Rye.Configuration;
using Rye.EventBus.Redis;
using Rye.EventBus.Redis.Extensions;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace Rye.Test.EventBus
{
    public class RedisEventBusTest
    {
        [Fact()]
        public async Task PushblishTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddRedisEventBus(options =>
                {
                    options.RedisOptions = redisOptions =>
                        ConfigurationManager.Appsettings.GetSection("Framework:Redis").GetChildren().FirstOrDefault().Bind(redisOptions);
                    options.ClientId = "";
                    options.Key = "RyeEventBus";
                });
            var services = serviceCollection.BuildServiceProvider();

            var eventBus = services.GetRequiredService<IRedisEventBus>();
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
