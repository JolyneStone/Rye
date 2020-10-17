using Microsoft.Extensions.DependencyInjection;

using Monica.Module;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monica
{
    public static class ModuleServiceProvider
    {
        /// <summary>
        /// 启用模块配置
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseModule(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return null;
            var modules = serviceProvider.GetServices<IStartupModule>();
            if (modules == null || !modules.Any())
                return serviceProvider;

            foreach (var module in modules.OrderBy(d => d, new ModuleComparer()))
            {
                module.Configure(serviceProvider);
            }

            return serviceProvider;
        }
    }
}
