using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Rye.Cache.Redis;
using Rye.Configuration;
using Rye.DependencyInjection;

using System;
using System.IO;
using System.Linq;

namespace Rye.Test
{
    public class TestSetup
    {
        public static IServiceProvider ConfigService(Action<IServiceCollection> configAction)
        {
            var devSetting = ConfigurationManager.Appsettings.GetSection("ASPNETCORE_ENVIRONMENT").Value == "Development" ? ".Development" : "";
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

            SingleServiceLocator.ConfigService(host.Services);
            return host.Services;
        }
    }
}
