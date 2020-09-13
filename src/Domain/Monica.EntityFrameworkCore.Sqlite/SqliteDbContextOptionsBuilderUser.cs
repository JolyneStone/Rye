using Microsoft.EntityFrameworkCore;
using Monica.Enums;

namespace Monica.EntityFrameworkCore.Sqlite
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
