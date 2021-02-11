using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Monica.Configuration;
using Monica.DependencyInjection;
using Monica.Logger;
using System;
using System.IO;
using Monica;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Monica.AspectFlare.DynamicProxy;
using Monica.EntityFrameworkCore.Options;

namespace Monica.Test
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
                        .AddCacheModule()
                        .ConfigureModule();
                }).Build();
        }
    }
}
