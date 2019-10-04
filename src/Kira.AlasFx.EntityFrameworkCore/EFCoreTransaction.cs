using Kira.AlasFx.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Kira.AlasFx.EntityFrameworkCore
{
    public class EFCoreTransaction : ITransaction
    {
        private DbTransaction _dbTransaction;
        private IDbContextTransaction _dbContextTransaction;

        public EFCoreTransaction(DbTransaction dbTransaction)
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
            await _dbTransaction?.CommitAsync(cancellationToken);
        }

        public void Rollback()
        {
            _dbContextTransaction?.Rollback();
            _dbTransaction?.Rollback();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction?.RollbackAsync(cancellationToken);
            await _dbTransaction?.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
            _dbTransaction?.Dispose();
        }
    }
}
