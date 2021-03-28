using Rye.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Rye.Enums;

namespace Rye.EntityFrameworkCore
{
    /// <summary>
    /// 数据库使用接口
    /// </summary>
    public interface IDbContextOptionsBuilderUser
    {
        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL等
        /// </summary>
        DatabaseType Type { get; }

        ///// <summary>
        ///// 使用数据库
        ///// </summary>
        ///// <param name="builder">创建器</param>
        ///// <param name="connectionString">连接字符串</param>
        ///// <returns></returns>
        //DbContextOptionsBuilder Use(DbContextOptionsBuilder builder, string connectionString);

        /// <summary>
        /// 使用数据库
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        DbContextOptionsBuilder<TDbContext> Use<TDbContext>(DbContextOptionsBuilder<TDbContext> builder, string connectionString)
            where TDbContext : DbContext, IDbContext;
    }
}
