using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Raven.DataAccess
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// 当前单元工作对象
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 当前数据上下文对象
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// 查询是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 异步查询是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 获取满足过滤条件的第一条数据,若不存在,则返回Null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 异步获取满足过滤条件的第一条数据,若不存在,则返回Null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 获取可查询集合, 若isTracking为true, 则返回可追踪集合, 否则返回不追踪集合, 默认为true
        /// </summary>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(bool isTracking = true);

        /// <summary>
        /// 获取可查询集合并进行过滤, 若isTracking为true, 则返回可追踪集合, 否则返回不追踪集合, 默认为true
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool isTracking = true);
    }

    public interface IReadOnlyRepository<TEntity, TKey>: IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
    }

    /// <summary>
    /// 无主键实体的仓储对象接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities"></param>
        void BatchDelete(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步批量删除实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchDeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除过滤后的实体
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步删除过滤后的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities"></param>
        void BatchInsert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步批量插入实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchInsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// 异步插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities"></param>
        void BatchUpdate(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步批量更新实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchUpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="updateExpression"></param>
        void Update(Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task UpdateAsync(Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 更新过滤后的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 异步更新过滤后的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);
    }

    /// <summary>
    /// 实体仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}
