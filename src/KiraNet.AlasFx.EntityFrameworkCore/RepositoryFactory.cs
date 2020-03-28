using KiraNet.AlasFx.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public RepositoryFactory()
        {
        }

        public IRepository<TEntity> GetRepository<TEntity>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity
        {
            return new Repository<TEntity>(unitOfWork);
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return new Repository<TEntity, TKey>(unitOfWork);
        }

        public IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>(IUnitOfWork unitOfWork)
             where TEntity : class, IEntity
        {
            return new ReadOnlyRepository<TEntity>(unitOfWork);
        }

        public IReadOnlyRepository<TEntity, TKey> GetReadOnlyRepository<TEntity, TKey>(IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return new ReadOnlyRepository<TEntity, TKey>(unitOfWork);
        }
    }
}
