using Monica.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Monica.Enums;

namespace Monica.EntityFrameworkCore
{
    /// <summary>
    /// 数据库使用接口
    /// </summary>
    public interface IDbContextOptionsBuilderUser
    {
        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL，SQLITE等
        /// </summary>
        DatabaseType Type { get; }

        /// <summary>
        /// 使用数据库
        /// </summary>
        /// <param name="builder">创建器</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        DbContextOptionsBuilder Use(DbContextOptionsBuilder builder, string connectionString);
    }
}
