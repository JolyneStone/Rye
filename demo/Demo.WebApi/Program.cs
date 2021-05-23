using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Rye;

namespace Demo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureApp()
                        .UseStartup<Startup>();
                        //.UseSerilogDefault(); // 添加Serilog 日志支持
                });
    }
}
