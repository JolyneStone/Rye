using Microsoft.Extensions.DependencyInjection;

using Monica.AspectFlare.DynamicProxy;
using Monica.DependencyInjection;

using System.Diagnostics;

using Xunit;

namespace Monica.Test.AOP
{
    public class AopTests
    {
        [Xunit.Fact()]
        public void Simple()
        {
            //var caller = new ReturnCaller<string>(default);
            //caller.Call(this, () => "123", null);
            var serviceCollection = new ServiceCollection()
              .UseDynamicProxyService()
              .AddScoped<ITest, TestClass>()
              .AddDistributedMemoryCache();

            SingleServiceLocator.SetServiceCollection(serviceCollection);
             var services = serviceCollection.BuildServiceProvider();

            var test1 = services.GetRequiredService<ITest>();
            var str = test1.Output(1);
            var str1 = test1.Output(1);
            //Assert.Equal("test", str);
            Debug.WriteLine("\n----------------------------------\n");

            var proxyProvider = services.GetRequiredService<IProxyProvider>();
            var test2 = proxyProvider.GetProxy<ITest, TestClass>();
            test2.Output(1);
            Assert.True(true);
        }
    }
}
