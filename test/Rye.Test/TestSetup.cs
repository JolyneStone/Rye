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
            var devSetting = App.Configuration.GetSection("ASPNETCORE_ENVIRONMENT").Value == "Development" ? ".Development" : "";
            var host = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(configure =>
                {
                    configure.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings{devSetting}.json")
                        .Build();

                })
                .ConfigureServices((context, services) =>
                {
                    configAction(services);
                }).Build();

            App.ConfigureServiceLocator(host.Services);
            return host.Services;
        }
    }
}
