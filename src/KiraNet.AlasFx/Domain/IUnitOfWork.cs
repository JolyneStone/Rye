using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KiraNet.AlasFx.Domain
{
    public interface IUnitOfWork : IDisposable 
    {
        /// <summary>
        /// 当前数据上下文对象
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// 获取只读仓储对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IEntity;

        /// <summary>
        /// 获取只读仓储对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity, TKey> GetReadOnlyRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// 获取仓储对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() 
            where TEntity : class, IEntity;

        /// <summary>
        /// 获取仓储对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// 开启事务
        /// </summary>
        ITransaction BeginOrUseTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, DbTransaction transaction = null);

        /// <summary>
        /// 异步开启事务
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<ITransaction> BeginOrUseTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, DbTransaction transaction = null, CancellationToken cancelToken = default);

        /// <summary>
        /// 保存数据变更 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 异步保存数据变更 
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancelToken = default);
    }
}
