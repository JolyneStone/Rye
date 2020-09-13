using Microsoft.EntityFrameworkCore;
using Monica.Enums;

namespace Monica.EntityFrameworkCore.SqlServer
{
    public class SqlServerDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.SqlServer;

        public DbContextOptionsBuilder Use(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseSqlServer(connectionString);
        }
    }
}
