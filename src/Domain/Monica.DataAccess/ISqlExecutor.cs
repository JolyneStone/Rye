using Monica.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monica.DataAccess
{
    /// <summary>
    /// 执行SQL语句
    /// </summary>
    public interface ISqlExecutor: IDisposable
    {
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        DatabaseType DatabaseType { get; }

        /// <summary>
        /// 查询指定SQL的结果集
        /// </summary>
        /// <typeparam name="TResult">结果集类型</typeparam>
        /// <param name="sql">查询的SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>结果集</returns>
        IEnumerable<TResult> FromSql<TResult>(string sql, object param = null);

        /// <summary>
        /// 执行指定的SQL语句
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>操作影响的行数</returns>
        int ExecuteSqlCommand(string sql, object param = null);
    }
}
