using Rye.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rye.SqlSugarCore.Init
{
    /// <summary>
    /// 表结构初始化
    /// </summary>
    public class TableScanning : RyeDbContext
    {
        private readonly TableFirst TF = new();

        #region 构造函数

        /// <summary>
        /// 查询指定的表进行初始化
        /// </summary>
        private readonly List<string> ClassNames = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        public TableScanning() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TableScanning(params string[] cname)
        {
            ClassNames = cname.ToList();
        }

        #endregion 构造函数

        /// <summary>
        /// 初始化开始
        /// </summary>
        public bool Init(bool tableFirst = false, params string[] dllPrefixs)
        {
            Console.WriteLine("表结构初始化开始!");

            foreach (var file in ReflectionHelper.GetFiles(dllPrefixs))
            {
                var assembly = Assembly.Load(file.Name.Replace(".dll", ""));
                var atemplate = assembly.GetTypes().Where(w => w.GetInterfaces().Contains(typeof(ISugarTableProvider)) && w.Name.EndsWith("Table"));
                if (ClassNames != null && ClassNames.Any())
                {
                    atemplate = atemplate.Where(w => ClassNames.Contains(w.Name));
                }
                var atype = atemplate.ToList();
                foreach (var atit in atype)
                {
                    //刷入表结构
                    try
                    {
                        Db.CodeFirst.InitTables(atit);
                        Console.WriteLine($"{file.Name} {atype.IndexOf(atit) + 1}/{atype.Count}");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "ORA-01442: 要修改为 NOT NULL 的列已经是 NOT NULL")
                        {
                            Console.WriteLine($"{file.Name} {atype.IndexOf(atit) + 1}/{atype.Count}");
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"{file.Name} {atype.IndexOf(atit) + 1}/{atype.Count}");
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }

                    //刷入表字典
                    if (tableFirst)
                    {
                        TF.InitTables(atit);
                    }
                }
            }

            Console.WriteLine("表结构初始化完成!");
            return true;
        }
    }
}
