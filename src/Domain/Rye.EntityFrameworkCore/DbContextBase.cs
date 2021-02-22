using Rye.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Rye.EntityFrameworkCore
{
    /// <summary>
    /// Rye框架的EF Core 上下文基类
    /// </summary>
    public abstract class DbContextBase<TContext> : DbContext, IDbContext
        where TContext: DbContext, IDbContext
    {
        public DbContextBase()
        {

        }

        public DbContextBase(DbContextOptions<TContext> options) : base(options)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
