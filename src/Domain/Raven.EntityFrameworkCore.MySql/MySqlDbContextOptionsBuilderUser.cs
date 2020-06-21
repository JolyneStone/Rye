using Raven.DataAccess;
using Microsoft.EntityFrameworkCore;
using Raven.Enums;

namespace Raven.EntityFrameworkCore.MySql
{
    public class MySqlDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.MySql;

        public DbContextOptionsBuilder Use(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseMySQL(connectionString);
        }
    }
}
