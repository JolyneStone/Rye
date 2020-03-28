using Microsoft.EntityFrameworkCore;
using System;

namespace KiraNet.AlasFx.EntityFrameworkCore.Options
{
    /// <summary>
    /// DbContextOptionsBuilder配置选项
    /// </summary>
    public class DbContextOptionsBuilderOptions
    {
        /// <summary>
        /// 配置DbContextOptionsBuilder, dbName指定数据库名称, 为null时表示所有数据库,默认为null
        /// </summary>
        /// <param name="build"></param>
        /// <param name="dbName"></param>
        /// <param name="dbContextType"></param>
        public DbContextOptionsBuilderOptions(DbContextOptionsBuilder build, string dbName = null, Type dbContextType = null)
        {
            Builder = build;
            DbName = dbName;
            DbContextType = dbContextType;
        }

        public DbContextOptionsBuilder Builder { get; }
        public string DbName { get; }
        public Type DbContextType { get; }
    }
}
