using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Rye.Logger;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Serilog 日志拓展
    /// </summary>
    public static class SerilogHostingExtensions
    {
        /// <summary>
        /// 添加Serilog默认日志拓展
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="configAction"></param>
        /// <returns>IWebHostBuilder</returns>
        public static WebApplicationBuilder UseSerilogDefault(this WebApplicationBuilder hostBuilder, Action<LoggerConfiguration> configAction = default)
        {
            // 加载配置文件
            var config = new LoggerConfiguration()
                .ReadFrom.Configuration(hostBuilder.Configuration)
                .Enrich.FromLogContext();
            if (configAction != null) configAction.Invoke(config);
            else
            {
                config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                      .WriteTo.File(Path.Combine("logs", "application.log"), LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit: null, encoding: Encoding.UTF8);
            }


            if (configAction != null) configAction.Invoke(config);
            else
            {
                config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                      .WriteTo.File(Path.Combine("logs", "application.log"), LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit: null, encoding: Encoding.UTF8);
            }

            //Serilog.Log.Logger = config.CreateLogger();
            Rye.Log.Current = new SerilogStaticLog();

            return hostBuilder;
        }

        /// <summary>
        /// 添加默认日志拓展
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IHostBuilder UseSerilogDefault(this IHostBuilder builder, Action<LoggerConfiguration> configAction = default)
        {
            builder.UseSerilog((context, configuration) =>
            {
                // 加载配置文件
                var config = configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext();

                if (configAction != null) configAction.Invoke(config);
                else
                {
                    config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                          .WriteTo.File(Path.Combine("logs", "application.log"), LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit: null, encoding: Encoding.UTF8);
                }

                //Serilog.Log.Logger = config.CreateLogger();
                Rye.Log.Current = new SerilogStaticLog();
            });

            return builder;
        }
    }
}