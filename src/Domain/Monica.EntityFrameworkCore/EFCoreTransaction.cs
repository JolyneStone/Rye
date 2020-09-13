using Monica.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Monica.EntityFrameworkCore
{
    public class EFCoreTransaction : ITransaction
    {
        private IDbTransaction _dbTransaction;
        private IDbContextTransaction _dbContextTransaction;

        public EFCoreTransaction(IDbTransaction dbTransaction)
        {
            Check.NotNull(dbTransaction, nameof(DbTransaction));
            _dbTransaction = dbTransaction;
        }

        public EFCoreTransaction(IDbContextTransaction dbContextTransaction)
        {
            Check.NotNull(dbContextTransaction, nameof(dbContextTransaction));
            _dbContextTransaction = dbContextTransaction;
        }

        public void Commit()
        {
            _dbContextTransaction?.Commit();
            _dbTransaction?.Commit();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction?.CommitAsync(cancellationToken);
            if (_dbTransaction is DbTransaction transaction)
            {
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                _dbTransaction?.Commit();
            }
        }

        public void Rollback()
        {
            _dbContextTransaction?.Rollback();
            _dbTransaction?.Rollback();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction?.RollbackAsync(cancellationToken);
            if (_dbTransaction is DbTransaction transaction)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            else
            {
                _dbTransaction?.Commit();
            }
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
            _dbTransaction?.Dispose();
        }
    }
}
