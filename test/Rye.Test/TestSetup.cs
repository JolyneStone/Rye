using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.IO;

namespace Rye.Test
{
    public class TestSetup
    {
        public static IServiceProvider ConfigService(Action<IServiceCollection> configAction)
        {
            var host = Host.CreateDefaultBuilder(null)
               .ConfigureApp()
               .ConfigureServices((context, services) =>
                {
                    configAction(services);
                }).Build();

            App.ConfigureServiceLocator(host.Services);
            return host.Services;
        }
    }
}
