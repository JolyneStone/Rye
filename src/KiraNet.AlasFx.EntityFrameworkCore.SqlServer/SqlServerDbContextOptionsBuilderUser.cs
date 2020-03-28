using KiraNet.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace KiraNet.AlasFx.EntityFrameworkCore.SqlServer
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
