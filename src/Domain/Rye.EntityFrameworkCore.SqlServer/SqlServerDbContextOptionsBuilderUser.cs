using Microsoft.EntityFrameworkCore;
using Rye.Enums;

namespace Rye.EntityFrameworkCore.SqlServer
{
    public class SqlServerDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.SqlServer;

        public DbContextOptionsBuilder<TDbContext> Use<TDbContext>(DbContextOptionsBuilder<TDbContext> builder, string connectionString)
            where TDbContext : DbContext, IDbContext
        {
            return builder.UseSqlServer(connectionString);
        }
    }
}
