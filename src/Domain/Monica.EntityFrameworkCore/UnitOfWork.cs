using Monica.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monica.EntityFrameworkCore
{
    /// <summary>
    /// UnitOfWork默认实现类
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbContext _context;
        private readonly DbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private bool _disposed;

        public UnitOfWork(IServiceProvider serviceProvider, IDbContext context)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            _serviceProvider = serviceProvider;
            _context = context;
            _dbContext = _context.AsDbContext();
            _repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
        }

        public IDbContext DbContext => _context;

        public ITransaction BeginOrUseTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, DbTransaction transaction = null)
        {
            if (_dbContext == null)
                return null;

            if (transaction != null)
            {
                return new EFCoreTransaction(_dbContext.Database.UseTransaction(transaction));
            }
            else
            {
                return new EFCoreTransaction(_dbContext.Database.BeginTransaction(isolationLevel));
            }
        }

        public async Task<ITransaction> BeginOrUseTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, DbTransaction transaction = null, CancellationToken cancelToken = default)
        {
            if (_dbContext == null)
                return null;

            if (transaction != null)
            {
                return new EFCoreTransaction(await _dbContext.Database.UseTransactionAsync(transaction, cancelToken));
            }
            else
            {
                return new EFCoreTransaction(await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancelToken));
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _context?.Dispose();
            _disposed = true;
        }

        public void CleanChanges()
        {
            var entries = _dbContext.ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        public void CleanChanges<TEntity>()
            where TEntity : class, IEntity
        {
            var entries = _dbContext.ChangeTracker.Entries<TEntity>().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancelToken = default)
        {
            return await _context.SaveChangesAsync();
        }

        public IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IEntity
        {
            return _repositoryFactory.GetReadOnlyRepository<TEntity>(this);
        }

        public IReadOnlyRepository<TEntity, TKey> GetReadOnlyRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _repositoryFactory.GetReadOnlyRepository<TEntity, TKey>(this);
        }

        public IRepository<TEntity> GetRepository<TEntity>()
                where TEntity : class, IEntity
        {
            return _repositoryFactory.GetRepository<TEntity>(this);
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _repositoryFactory.GetRepository<TEntity, TKey>(this);
        }
    }
}
