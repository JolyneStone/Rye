using Microsoft.AspNetCore.Hosting;

namespace Rye
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureApp(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                App.HostEnvironment = hostingContext.HostingEnvironment;
                WebApp.WebHostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                App.AddConfigureFiles(config, hostingContext.HostingEnvironment);
            })
            .ConfigureServices(services =>
            {
                
            });
        }
    }
}
