using System;

namespace KiraNet.AlasFx.Domain
{
    /// <summary>
    /// 仓储工厂接口
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// 获取只读仓储对象
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity;

        /// <summary>
        /// 获取只读仓储对象
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity, TKey> GetReadOnlyRepository<TEntity, TKey>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// 获取仓储对象
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity;

        /// <summary>
        /// 获取仓储对象
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

    }
}
