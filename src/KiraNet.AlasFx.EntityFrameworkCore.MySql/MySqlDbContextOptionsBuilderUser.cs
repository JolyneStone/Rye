using KiraNet.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace KiraNet.AlasFx.EntityFrameworkCore.MySql
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
