using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rye.Module;
using Rye.Web.Module;
using System.Linq;

namespace Rye.Web
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用模块配置
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseModule(this IApplicationBuilder app)
        {
            if (app == null)
                return null;
            var modules = app.ApplicationServices.GetServices<IStartupModule>();
            if (modules == null || !modules.Any())
                return app;

            foreach (var module in modules.OrderBy(d => d, new ModuleComparer()))
            {
                if (module is AspNetCoreModule aspModule)
                    aspModule.Configure(app);
                else
                    module.Configure(app.ApplicationServices);
            }

            return app;
        }
    }
}
