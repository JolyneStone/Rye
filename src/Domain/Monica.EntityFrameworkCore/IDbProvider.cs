using Microsoft.EntityFrameworkCore;

using System;

namespace Monica.EntityFrameworkCore
{
    /// <summary>
    /// 数据库提供者接口
    /// </summary>
    public interface IDbProvider: IDisposable
    {
        /// <summary>
        /// 根据上下文类型及数据库名称获取UnitOfWork对象, dbName为null时默认为第一个数据库名称
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        IUnitOfWork GetUnitOfWork<TDbContext>(string dbName = null)
            where TDbContext : DbContext, IDbContext;
    }
}
