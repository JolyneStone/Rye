using Microsoft.EntityFrameworkCore;
using Rye.Enums;

namespace Rye.EntityFrameworkCore.Sqlite
{
    public class SqliteDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.Sqlite;

        public DbContextOptionsBuilder<TDbContext> Use<TDbContext>(DbContextOptionsBuilder<TDbContext> builder, string connectionString)
                where TDbContext : DbContext, IDbContext
        {
            return builder.UseSqlite(connectionString);
        }
    }
}
