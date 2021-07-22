using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Rye.Module;
using Rye.Web.Filter;
using Rye.Web.Module;
using Rye.Web.Options;

using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Rye.Web
{
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebRye(this IServiceCollection serviceCollection, Action<RyeWebOptions> action = null)
        {
            serviceCollection.AddHttpContextAccessor();
            if (action == null)
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                action = options => serviceProvider.GetService<IConfiguration>().GetSection("Framework:Web").Bind(options);
            }
            serviceCollection.Configure<RyeWebOptions>(action);
            AddWebRyeCore(serviceCollection);
            return serviceCollection;
        }

        private static void AddWebRyeCore(IServiceCollection serviceCollection)
        {
            using (var scopeProvider = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var webOptions = scopeProvider.ServiceProvider.GetRequiredService<IOptions<RyeWebOptions>>().Value;
                serviceCollection.AddControllers(options =>
                {
                    if (webOptions.GlobalActionFilter.Enabled)
                    {
                        options.Filters.Add<GlobalActionFilter>();
                    }

                    if (webOptions.ModeValidation.Enabled)
                    {
                        //关闭默认检查
                        serviceCollection.Configure<ApiBehaviorOptions>(options =>
                        {
                            options.SuppressModelStateInvalidFilter = true;
                        });

                        options.Filters.Add<ModelValidationAttribute>();
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
                });
            }
        }

        /// <summary>
        /// 添加Rye框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebModule(this IServiceCollection serviceCollection, Action<RyeWebOptions> action = null)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            var module = new AspNetCoreConfigModule(action);
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupModule>(module));
            return serviceCollection;
        }
    }
}
