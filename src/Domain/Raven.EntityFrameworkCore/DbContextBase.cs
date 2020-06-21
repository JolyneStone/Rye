using Raven.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Raven.EntityFrameworkCore
{
    /// <summary>
    /// Raven框架的EF Core 上下文基类
    /// </summary>
    public abstract class DbContextBase : DbContext, IDbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
