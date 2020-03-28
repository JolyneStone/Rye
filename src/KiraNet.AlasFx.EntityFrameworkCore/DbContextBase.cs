using KiraNet.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    /// <summary>
    /// Alas框架的EF Core 上下文基类
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
