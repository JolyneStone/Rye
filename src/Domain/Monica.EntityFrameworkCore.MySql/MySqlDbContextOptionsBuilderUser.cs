using Microsoft.EntityFrameworkCore;
using Monica.Enums;

namespace Monica.EntityFrameworkCore.MySql
{
    public class MySqlDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.MySql;

        public DbContextOptionsBuilder Use(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
