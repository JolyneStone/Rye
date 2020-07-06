using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Raven.Configuration;
using Raven.DependencyInjection;
using Raven.Log;
using System;
using System.IO;

namespace Raven.Test
{
    public class TestSetup
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static void ConfigService()
        {
            var devSetting = ConfigurationManager.Appsettings.GetSection("ASPNETCORE_ENVIRONMENT").Value == "Development" ? ".Development" : "";
            Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(configure =>
                {
                    configure.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings{devSetting}.json")
                        .Build();

                })
                .ConfigureServices((context, services) =>
                {
                    services.AddRaven()
                        .AddRavenLog();
                    SingleServiceLocator.SetServiceCollection(services);
                }).Build();
        }
    }
}
