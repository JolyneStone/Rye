//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Rye
//{
//    public static class Rye
//    {
//        /// <summary>
//        /// 添加配置文件
//        /// </summary>
//        /// <param name="config"></param>
//        /// <param name="env"></param>
//        public static void AddConfigureFiles(IConfigurationBuilder config, IHostEnvironment env)
//        {
//            var appsettingsConfiguration = config.Build();
//            // 读取忽略的配置文件
//            var ignoreConfigurationFiles = appsettingsConfiguration
//                    .GetSection("IgnoreConfigurationFiles")
//                    .Get<string[]>()
//                ?? Array.Empty<string>();

//            // 加载配置
//            AutoAddJsonFiles(config, env, ignoreConfigurationFiles);

//            // 存储配置
//            Configuration = config.Build();
//        }
//    }
//}
