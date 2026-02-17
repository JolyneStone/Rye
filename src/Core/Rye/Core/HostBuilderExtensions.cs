using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Rye
{
    public static class HostBuilderExtensions
    {
        public static IHostApplicationBuilder ConfigureApp(this IHostApplicationBuilder builder)
        {
            // 存储环境对象
            App.HostEnvironment = builder.Environment;

            // 加载配置
            App.AddConfigureFiles(builder.Configuration, builder.Environment);

            builder.Services.AddRye();

            return builder;
        }

        public static IHostApplicationBuilder ConfigureApp(this IHostApplicationBuilder builder, Action<IServiceCollection> action)
        {
            // 存储环境对象
            App.HostEnvironment = builder.Environment;

            // 加载配置
            App.AddConfigureFiles(builder.Configuration, builder.Environment);

            builder.Services.AddRye();
            if(action != null)
            {
                action(builder.Services);
            }
            return builder;
        }

        public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((context, builder) =>
            {
                // 存储环境对象
                App.HostEnvironment = context.HostingEnvironment;

                // 加载配置
                App.AddConfigureFiles(builder, context.HostingEnvironment);

            })
             .ConfigureServices(services =>
             {
                 services.AddRye();
             });
        }

        public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder builder, Action<IServiceCollection> action)
        {
            return builder.ConfigureAppConfiguration((context, builder) =>
             {
                 // 存储环境对象
                 App.HostEnvironment = context.HostingEnvironment;

                 // 加载配置
                 App.AddConfigureFiles(builder, context.HostingEnvironment);

             })
             .ConfigureServices(services =>
             {
                 services.AddRye();
                 if(action != null)
                 {
                     action(services);
                 }
             });
        }

        //public static IHostBuilder ConfigureApp(HostBuilderContext context, IServiceCollection service)
        //{
        //    // 存储环境对象
        //    App.HostEnvironment = context.HostingEnvironment;
        //    App.Configuration = context.Configuration;
        //    // 加载配置
        //    App.AddConfigureFiles(new , App.HostEnvironment);
        //    builder.ConfigureServices((context, services) =>
        //    {
        //        services.AddRye();
        //    });
        //    return builder;
        //}
    }
}
