using Kira.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kira.AlasFx.EntityFrameworkCore.Sqlite
{
    public class SqliteDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.Sqlite;

        public DbContextOptionsBuilder Use(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseSqlite(connectionString);
        }
    }
}
