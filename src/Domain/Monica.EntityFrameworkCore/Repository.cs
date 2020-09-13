using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Monica.DataAccess;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Runtime.CompilerServices;

namespace Monica.EntityFrameworkCore
{
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IDbContext _context;
        protected readonly DbSet<TEntity> _set;

        public ReadOnlyRepository(IUnitOfWork unitOfWork)
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));

            _unitOfWork = unitOfWork;
            _context = unitOfWork.DbContext;
            _set = _context.AsDbContext().Set<TEntity>();
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public IDbContext DbContext => _context;

        public bool Exists(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return _set.Any();
            else
                return _set.Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return await _set.AnyAsync();
            else
                return await _set.AnyAsync(predicate);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return _set.FirstOrDefault();
            else
                return _set.FirstOrDefault(predicate);
        }

        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return _set.FirstOrDefaultAsync();
            else
                return _set.FirstOrDefaultAsync(predicate);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return _set.Count();
            else
                return _set.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return await _set.CountAsync();
            else
                return await _set.CountAsync(predicate);
        }

        public IQueryable<TEntity> Query(bool isTracking = true)
        {
            return isTracking ? _set : _set.AsNoTracking();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool isTracking = true)
        {
            Check.NotNull(predicate, nameof(predicate));
            return (isTracking ? _set : _set.AsNoTracking()).Where(predicate);
        }
    }

    public class ReadOnlyRepository<TEntity, TKey> : ReadOnlyRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public ReadOnlyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public class Repository<TEntity> : ReadOnlyRepository<TEntity>, IRepository<TEntity>
        where TEntity : class, IEntity
    {
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Delete(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            _set.Remove(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            _set.Remove(entity);
            await Task.CompletedTask;
        }

        public void BatchDelete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                _set.BatchDelete();
            else 
                _set.Where(predicate).BatchDelete();
        }

        public async Task BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                await _set.BatchDeleteAsync();
            else
                await _set.Where(predicate).BatchDeleteAsync();
        }

        public void BatchInsert(IList<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            _context.AsDbContext().BulkInsert(entities);
        }

        public async Task BatchInsertAsync(IList<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            await _context.AsDbContext().BulkInsertAsync(entities);
        }

        public void Insert(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            _set.Add(entity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            await _set.AddAsync(entity);
        }

        public void BatchUpdate(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(updateExpression, nameof(updateExpression));
            _set.BatchUpdate(updateExpression);
        }

        public async Task BatchUpdateAsync(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(updateExpression, nameof(updateExpression));
            await _set.BatchUpdateAsync(updateExpression);
        }

        public void BatchUpdate(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));
            _set.Where(predicate).BatchUpdate(updateExpression);
        }

        public async Task BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));
            await _set.Where(predicate).BatchUpdateAsync(updateExpression);
        }

        public void Update(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            _set.Update(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            _set.Update(entity);
            await Task.CompletedTask;
        }
    }

    public class Repository<TEntity, TKey> : Repository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}