using Microsoft.Extensions.DependencyInjection;

using Rye.AspectFlare.DynamicProxy;
using Rye.DependencyInjection;

using System.Diagnostics;
using System.Threading.Tasks;

using Xunit;

namespace Rye.Test.AOP
{
    public class AopTests
    {
        [Xunit.Fact()]
        public async Task Simple()
        {
            //var caller = new ReturnCaller<string>(default);
            //caller.Call(this, () => "123", null);
            var serviceCollection = new ServiceCollection()
              .UseDynamicProxyService()
              .AddScoped<ITest, TestClass>()
              .AddDistributedMemoryCache();

             var services = serviceCollection.BuildServiceProvider();
            App.ConfigureServiceLocator(services);

            var test1 = services.GetRequiredService<ITest>();
            var str = await test1.Output(1);
            var str1 = await test1.Output(1);
            //Assert.Equal("test", str);
            Debug.WriteLine("\n----------------------------------\n");

            var proxyProvider = services.GetRequiredService<IProxyProvider>();
            var test2 = proxyProvider.GetProxy<ITest, TestClass>();
            await test2.Output(1);
            Assert.True(true);
        }
    }
}
