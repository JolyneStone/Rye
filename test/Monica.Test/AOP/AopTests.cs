using Microsoft.Extensions.DependencyInjection;
using Monica.AspectFlare.DependencyInjection;
using Monica.AspectFlare.DynamicProxy;
using System;
using System.Diagnostics;

using Xunit;

namespace Monica.Test.AOP
{
    public class AopTests
    {
        [Fact()]
        public void Simple()
        {
            var services = new ServiceCollection()
              .UseDynamicProxyService(true)
              .AddScoped<ITest, TestClass>()
              .BuildServiceProvider();

            var test1 = services.GetRequiredService<ITest>();
            test1.Output();

            Debug.WriteLine("\n----------------------------------\n");

            var proxyProvider = ProxyFlare.Flare.UseDefaultProviders(true).GetProvider();
            var test2 = proxyProvider.GetProxy<ITest, TestClass>();
            test2.Output();
            Assert.True(true);
        }
    }
}
