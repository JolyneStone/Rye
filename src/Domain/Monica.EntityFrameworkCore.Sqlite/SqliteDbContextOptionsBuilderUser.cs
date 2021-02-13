using Microsoft.EntityFrameworkCore;
using Monica.Enums;

namespace Monica.EntityFrameworkCore.Sqlite
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
