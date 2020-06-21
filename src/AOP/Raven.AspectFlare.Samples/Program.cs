using System;
using Raven.AspectFlare.DependencyInjection;
using Raven.AspectFlare.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Simples
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .UseDynamicProxyService(true)
                .AddScoped<ITest, TestClass>()
                .BuildServiceProvider();

            var test1 = services.GetRequiredService<ITest>();
            test1.Output();

            Console.WriteLine("\n----------------------------------\n");

            var proxyProvider = ProxyFlare.Flare.UseDefaultProviders(true).GetProvider();
            var test2= proxyProvider.GetProxy<ITest, TestClass>();
            test2.Output();
            Console.ReadKey();
        }
    }
}
