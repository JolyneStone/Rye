using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rye.Configuration
{
    /// <summary>
    /// 全局配置管理类
    /// </summary>
    public static class ConfigurationManager
    {
        public static IConfiguration GetConfiguration(string jsonFile, bool optional = true, bool reloadOnChange = true)
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(jsonFile, optional: optional, reloadOnChange: reloadOnChange)
                .Build();
        }

        public static bool TyeGetConfiguration(string jsonFile, out IConfiguration configuration, bool optional = true, bool reloadOnChange = true)
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (!File.Exists(Path.Combine(currentDirectory, jsonFile)))
            {
                configuration = null;
                return false;
            }
            configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(jsonFile, optional: optional, reloadOnChange: reloadOnChange)
                .Build();
            return true;
        }

        private static IConfiguration appsettings;
        public static IConfiguration Appsettings
        {
            get
            {
                if (appsettings == null)
                {

                    appsettings = GetConfiguration("appsettings.json");
                    string env = appsettings.GetSection("ASPNETCORE_ENVIRONMENT").Value;
                    if (!string.IsNullOrEmpty(env))
                    {
                        if (TyeGetConfiguration($"appsettings.{env}.json", out var newConfig))
                        {
                            appsettings = newConfig;
                        }
                    }
                }
                return appsettings;
            }
            set
            {
                if (appsettings != value)
                {
                    appsettings = value;
                }
            }
        }

        /// <summary>
        /// 获取json配置直接获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSectionValue(string key)
        {
            return Appsettings.GetSection(key).Value;
        }
        /// <summary>
        /// 获取json配置映射到模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSection<T>(string key) where T : class, new()
        {
            return Appsettings.GetSection(key).Get<T>();
        }
    }
}
