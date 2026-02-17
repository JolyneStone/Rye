using Rye.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rye.SqlSugarCore.Init
{
    /// <summary>
    /// 基础数据初始化
    /// </summary>
    public class InitScanning : RyeDbContext
    {
        #region 构造函数

        /// <summary>
        /// 查询指定的表进行初始化
        /// </summary>
        private readonly List<string> ClassNames = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        public InitScanning() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public InitScanning(params string[] cname)
        {
            ClassNames = cname.ToList();
        }

        #endregion 构造函数

        /// <summary>
        /// 初始化开始
        /// </summary>
        public async Task<bool> Init(params string[] dllPrefixs)
        {
            Console.WriteLine("");
            Console.WriteLine("数据初始化开始!");

            //遍历所有dll文件
            foreach (var file in ReflectionHelper.GetFiles(dllPrefixs))
            {
                var assembly = Assembly.Load(file.Name.Replace(".dll", ""));
                //获取dll文件中 文件名是 Init结尾的信息
                var atemplate = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISugarDataProvider)) && t.Name.EndsWith("Init"));
                if (ClassNames != null && ClassNames.Any())
                {
                    atemplate = atemplate.Where(w => ClassNames.Contains(w.Name));
                }
                var atype = atemplate.ToList();
                foreach (var atit in atype)
                {
                    try
                    {
                        var provider = Activator.CreateInstance(atit);
                        await ((ISugarDataProvider)provider).InitAsync();
                        Console.WriteLine($"{file.Name} {atype.IndexOf(atit) + 1}/{atype.Count}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{file.Name} {atype.IndexOf(atit) + 1}/{atype.Count}");
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine("数据初始化完成!");
            return true;
        }
    }
}
