using KiraNet.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace KiraNet.AlasFx.EntityFrameworkCore.Sqlite
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
