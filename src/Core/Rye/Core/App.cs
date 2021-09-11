using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;

using Rye;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace Rye
{
    public partial class App
    {
        /// <summary>
        /// 获取泛型主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IHostEnvironment HostEnvironment { get; internal set; }

        #region Configuration

        /// <summary>
        /// 添加配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="env"></param>
        internal static void AddConfigureFiles(IConfigurationBuilder config, IHostEnvironment env)
        {
            var appsettingsConfiguration = config.Build();
            // 读取忽略的配置文件
            var ignoreConfigurationFiles = appsettingsConfiguration
                    .GetSection("IgnoreConfigurationFiles")
                    .Get<string[]>()
                ?? Array.Empty<string>();

            // 加载配置
            AutoAddJsonFiles(config, env, ignoreConfigurationFiles);

            // 存储配置
            Configuration = config.Build();
        }

        /// <summary>
        /// 自动加载自定义 .json 配置文件
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <param name="env"></param>
        /// <param name="ignoreConfigurationFiles"></param>
        private static void AutoAddJsonFiles(IConfigurationBuilder configBuilder, IHostEnvironment env, string[] ignoreConfigurationFiles)
        {
            //// 获取服务运行的目录
            //var processModule = Process.GetCurrentProcess()?.MainModule;
            //if (processModule != null)
            //{
            //    var pathToExe = processModule.FileName;
            //    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            //    Directory.SetCurrentDirectory(pathToContentRoot);
            //}

            // 获取程序目录下的所有配置文件
            var jsonFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json", SearchOption.TopDirectoryOnly)
                .Where(u => CheckIncludeDefaultSettings(Path.GetFileName(u)) && 
                    !ignoreConfigurationFiles.Contains(Path.GetFileName(u)) && 
                    !runtimeJsonSuffixs.Any(j => u.EndsWith(j)));

            if (jsonFiles == null || !jsonFiles.Any())
            {
                return;
            }

            // 获取环境变量名
            var envName = env.EnvironmentName;

            var appsettings = jsonFiles.FirstOrDefault(d => Path.GetFileName(d) == "appsettings.json");
            if (appsettings != null)
            {
                var newConfigBuilder = new ConfigurationBuilder();
                var newEnvName = newConfigBuilder.AddJsonFile(appsettings).Build()
                    .GetSection("DOTNET_ENVIRONMENT")?.Value;
                if (!newEnvName.IsNullOrEmpty())
                {
                    envName = newEnvName;
                }
            }

            // 遍历所有配置分组
            foreach (var jsonFile in jsonFiles.Where(j =>
                                                {
                                                    var fileName = Path.GetFileNameWithoutExtension(j);
                                                    return fileName.IndexOf('.') < 0 || fileName.EndsWith($".{envName}");
                                                }).OrderBy(j => Path.GetFileName(j).Length))
            {
                configBuilder.AddJsonFile(jsonFile, optional: true, reloadOnChange: true);
            }
        }

        /// <summary>
        /// 检查是否受排除的配置文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool CheckIncludeDefaultSettings(string fileName)
        {
            var isTrue = true;

            foreach (var prefix in excludeJsonPrefixs)
            {
                var isMatch = Regex.IsMatch(fileName, string.Format(excludeJsonPattern, prefix));
                if (isMatch)
                {
                    isTrue = false;
                    break;
                }
            }

            return isTrue;
        }

        /// <summary>
        /// 排序的配置文件前缀
        /// </summary>
        private static readonly string[] excludeJsonPrefixs = new[] { "bundleconfig", "compilerconfig" };

        /// <summary>
        /// 排除特定配置文件正则表达式
        /// </summary>
        private const string excludeJsonPattern = @"^{0}(\.\w+)?\.((json)|(xml))$";

        /// <summary>
        /// 排除运行时 Json 后缀
        /// </summary>
        private static readonly string[] runtimeJsonSuffixs = new[]
        {
            "deps.json",
            "runtimeconfig.dev.json",
            "runtimeconfig.prod.json",
            "runtimeconfig.json"
        };

        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Configuration { get; internal set; }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="jsonKey">配置中对应的Key</param>
        /// <returns>TOptions</returns>
        public static TOptions GetConfig<TOptions>(string jsonKey)
        {
            return Configuration.GetSection(jsonKey).Get<TOptions>();
        }

        #endregion

        #region 依赖注入

        public static IServiceProvider ApplicationServices { get; internal set; }

        /// <summary>
        /// 配置服务定位器
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider ConfigureServiceLocator(IServiceProvider serviceProvider)
        {
            App.ApplicationServices = serviceProvider;
            return serviceProvider;
        }


        public static IServiceScope CreateScope()
        {
            return App.ApplicationServices.CreateScope();
        }

        public static object GetRequiredService(Type serviceType)
        {
            return App.ApplicationServices.GetRequiredService(serviceType);
        }

        public static T GetRequiredService<T>() where T : notnull
        {
            return App.ApplicationServices.GetRequiredService<T>();
        }

#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
        public static T? GetService<T>()
#pragma warning restore CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
        {
            return App.ApplicationServices.GetService<T>();
        }

        public static IEnumerable<T> GetServices<T>()
        {
            return App.ApplicationServices.GetServices<T>();
        }

#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
        public static IEnumerable<object?> GetServices(Type serviceType)
#pragma warning restore CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
        {
            return App.ApplicationServices.GetServices(serviceType);
        }

        #endregion

        #region Reflection

        public static Assembly[] Assemblies { get; internal set; }
        public static Type[] ScanTypes { get; internal set; }

        #endregion
    }
}
