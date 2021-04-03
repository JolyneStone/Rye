using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rye.Configuration;
using Rye.DependencyInjection;
using Rye.Logger;
using System;
using System.IO;
using Rye;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Rye.AspectFlare.DynamicProxy;
using Rye.EntityFrameworkCore.Options;
using System.Linq;

namespace Rye.Test
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
                    services = services.UseDynamicProxyService();
                    services
                        .AddCoreModule()
                        .AddRedisCacheModule(options =>
                            ConfigurationManager.Appsettings.GetSection("Framework:Redis").GetChildren().FirstOrDefault().Bind(options))
                        .ConfigureModule();
                }).Build();
        }
    }
}
