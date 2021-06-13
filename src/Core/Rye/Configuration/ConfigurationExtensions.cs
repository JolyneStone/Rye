using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye
{
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// 获取json配置直接获取值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSectionValue(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Value;
        }
        /// <summary>
        /// 获取json配置映射到模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSection<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T>();
        }
    }
}
