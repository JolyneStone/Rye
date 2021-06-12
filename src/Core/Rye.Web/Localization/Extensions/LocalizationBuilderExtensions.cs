using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.Localization;

using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Rye
{
    public static class LocalizationBuilderExtensions
    {
        /// <summary>
        /// 配置多语言服务，
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddAppLocalization(this IMvcBuilder builder)
        {
            // 获取多语言配置选项
            using var serviceProvider = builder.Services.BuildServiceProvider();
            var localizationOptions = serviceProvider.GetService<IOptions<LocalizationSettingOptions>>().Value;
            // 注册请求多语言配置选项
            builder.Services.Configure((Action<RequestLocalizationOptions>)(options =>
            {
                // 如果设置了默认语言，则取默认语言，否则取第一个
                options.SetDefaultCulture(string.IsNullOrWhiteSpace(localizationOptions.DefaultCulture) ? localizationOptions.SupportedCultures[0] : localizationOptions.DefaultCulture)
                       .AddSupportedCultures(localizationOptions.SupportedCultures)
                       .AddSupportedUICultures(localizationOptions.SupportedCultures);

                // 自动根据客户端浏览器的语言实现多语言机制
                options.ApplyCurrentCultureToResponseHeaders = true;
            }));

            // 处理多语言在 Razor 视图中文乱码问题
            builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            // 配置视图多语言和验证多语言
            builder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                     .AddDataAnnotationsLocalization(options =>
                     {
                         options.DataAnnotationLocalizerProvider = (type, factory) =>
                             factory.Create(I18n.LangType);
                     });

            I18n.OnGetSelectCulturing = () =>
            {
                var httpContext = HttpContextUtil.GetCurrentHttpContext();
                if (httpContext == null) return default;

                // 获取请求特性
                var requestCulture = httpContext.Features.Get<IRequestCultureFeature>();
                return requestCulture.RequestCulture?.Culture;
            };

            I18n.OnSetCultured = (string culture) =>
            {
                var httpContext = HttpContextUtil.GetCurrentHttpContext();
                if (httpContext == null) return;

                httpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            };

            return builder;
        }

        public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
        {
            LocalizationSettingOptions value = app.ApplicationServices.GetRequiredService<IOptions<LocalizationSettingOptions>>().Value;
            if (value.SupportedCultures == null || value.SupportedCultures.Length == 0)
            {
                return app;
            }

            RequestLocalizationOptions requestLocalizationOptions = new RequestLocalizationOptions();
            // 如果设置了默认语言，则取默认语言，否则取第一个
            requestLocalizationOptions.SetDefaultCulture(string.IsNullOrWhiteSpace(value.DefaultCulture) ? value.SupportedCultures[0] : value.DefaultCulture)
                   .AddSupportedCultures(value.SupportedCultures)
                   .AddSupportedUICultures(value.SupportedCultures);

            // 自动根据客户端浏览器的语言实现多语言机制
            requestLocalizationOptions.ApplyCurrentCultureToResponseHeaders = true;
            app.UseRequestLocalization(requestLocalizationOptions);
            return app;
        }
    }
}
