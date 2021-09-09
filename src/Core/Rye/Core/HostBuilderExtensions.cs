using Microsoft.Extensions.Hosting;

namespace Rye
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureApp(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                App.HostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                App.AddConfigureFiles(config, hostingContext.HostingEnvironment);

                hostingContext.Configuration = App.Configuration;
            })
            .ConfigureServices(services => services.AddRye());
        }
    }
}
