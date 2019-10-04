using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kira.AlasFx.Domain;
using Kira.AlasFx.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Kira.AlasFx.EntityFrameworkCore
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

        public TEntity Get(TKey key)
        {
            return _set.FirstOrDefault(entity => key.Equals(entity.Key));
        }

        public async Task<TEntity> GetAsync(TKey key)
        {
            return await _set.FirstOrDefaultAsync(entity => key.Equals(entity.Key));
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

        public void BatchDelete(IEnumerable<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            _set.BulkDelete(entities);
        }

        public async Task BatchDeleteAsync(IEnumerable<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            await _set.BulkDeleteAsync(entities);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));
            _set.Where(predicate).Delete();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));
            await _set.Where(predicate).DeleteAsync();
        }

        public void BatchInsert(IEnumerable<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            _set.BulkInsert(entities);
        }

        public async Task BatchInsertAsync(IEnumerable<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            await _set.BulkInsertAsync(entities);
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

        public void BatchUpdate(IEnumerable<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            _set.BulkUpdate(entities);
        }

        public async Task BatchUpdateAsync(IEnumerable<TEntity> entities)
        {
            Check.NotNull(entities, nameof(entities));
            await _set.BulkUpdateAsync(entities);
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

        public void Update(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(updateExpression, nameof(updateExpression));
            _set.Update(updateExpression);
        }

        public async Task UpdateAsync(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(updateExpression, nameof(updateExpression));
            await _set.UpdateAsync(updateExpression);
        }

        public void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));
            _set.Where(predicate).Update(updateExpression);
        }

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));
            await _set.Where(predicate).UpdateAsync(updateExpression);
        }
    }

    public class Repository<TEntity, TKey> : Repository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void BatchDelete(IEnumerable<TKey> keys)
        {
            Check.NotNull(keys, nameof(keys));
            _set.Where(entity => keys.Contains(entity.Key)).Delete();
        }

        public async Task BatchDeleteAsync(IEnumerable<TKey> keys)
        {
            Check.NotNull(keys, nameof(keys));
            await _set.Where(entity => keys.Contains(entity.Key)).DeleteAsync();
        }

        public void Delete(TKey key)
        {
            var entity = _set.FirstOrDefault(entity => key.Equals(entity.Key));
            if(entity != null)
            {
                _set.Remove(entity);
            }
        }

        public async Task DeleteAsync(TKey key)
        {
            var entity = _set.FirstOrDefault(entity => key.Equals(entity.Key));
            if (entity != null)
            {
                _set.Remove(entity);
            }

            await Task.CompletedTask;
        }

        public TEntity Get(TKey key)
        {
            return _set.FirstOrDefault(entity => key.Equals(entity.Key));
        }

        public async Task<TEntity> GetAsync(TKey key)
        {
            return await _set.FirstOrDefaultAsync(entity => key.Equals(entity.Key));
        }
    }
}