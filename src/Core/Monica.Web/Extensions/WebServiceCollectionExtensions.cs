using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Monica.Module;
using Monica.Web.Filter;
using Monica.Web.Module;
using Monica.Web.Options;
using Monica.Web.Util;

using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Monica.Web
{
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebMonica(this IServiceCollection serviceCollection, Action<MonicaWebOptions> action = null)
        {
            serviceCollection.AddHttpContextAccessor();
            if (action == null)
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                action = options => serviceProvider.GetService<IConfiguration>().GetSection("Framework:Web").Bind(options);
            }
            serviceCollection.Configure<MonicaWebOptions>(action);
            AddWebMonicaCore(serviceCollection);
            return serviceCollection;
        }

        private static void AddWebMonicaCore(IServiceCollection serviceCollection)
        {
            using (var scopeProvider = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var webOptions = scopeProvider.ServiceProvider.GetRequiredService<IOptions<MonicaWebOptions>>().Value;
                serviceCollection.AddMvc(options =>
                {
                    if (webOptions.GlobalExceptionFilter.Enabled)
                    {
                        options.Filters.Add<GlobalExceptionFilter>();
                    }
                    if (webOptions.GlobalActionFilter.Enabled)
                    {
                        options.Filters.Add<GlobalActionFilter>();
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                });
            }
        }

        /// <summary>
        /// 添加Monica框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebModule(this IServiceCollection serviceCollection, Action<MonicaWebOptions> action = null)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            var module = new AspNetCoreMonincaModule(action);
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupModule>(module));
            return serviceCollection;
        }
    }
}
