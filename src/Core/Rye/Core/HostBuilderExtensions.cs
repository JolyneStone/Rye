using Microsoft.Extensions.Hosting;

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
    }
}
