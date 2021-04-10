using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Rye.Cache.Redis;
using Rye.Configuration;

using System;
using System.IO;
using System.Linq;

namespace Rye.Test
{
    public class TestSetup
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static void ConfigService()
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
                    services = services.UseDynamicProxyService();
                    services
                        .AddCoreModule()
                        .AddRedisCacheModule(options =>
                            ConfigurationManager.Appsettings.GetSection("Framework:Redis").GetChildren().FirstOrDefault().Bind(options))
                        .ConfigureModule();
                }).Build();

            ServiceProvider = host.Services;
        }
    }
}
