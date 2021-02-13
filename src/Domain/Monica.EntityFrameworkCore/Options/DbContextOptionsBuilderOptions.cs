using Microsoft.EntityFrameworkCore;

using Monica.DataAccess;

using System;

namespace Monica.EntityFrameworkCore.Options
{
    /// <summary>
    /// DbContextOptionsBuilder配置选项
    /// </summary>
    public class DbContextOptionsBuilderOptions<TDbContext>
        where TDbContext: DbContext, IDbContext
    {
        /// <summary>
        /// 配置DbContextOptionsBuilder, dbName指定数据库名称, 为null时表示所有数据库,默认为null
        /// </summary>
        public DbContextOptionsBuilderOptions()
        {
        }

        /// <summary>
        /// 配置DbContextOptionsBuilder, dbName指定数据库名称, 为null时表示所有数据库,默认为null
        /// </summary>
        /// <param name="build"></param>
        /// <param name="dbName"></param>
        /// <param name="dbContextType"></param>
        public DbContextOptionsBuilderOptions(DbContextOptionsBuilder<TDbContext> build, string dbName = null)
        {
            Builder = build;
            DbName = dbName;
        }

        public DbContextOptionsBuilder<TDbContext> Builder { get; set; }
        public string DbName { get; set; }
    }
}
