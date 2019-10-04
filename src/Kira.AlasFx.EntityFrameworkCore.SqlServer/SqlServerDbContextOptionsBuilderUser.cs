using Kira.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kira.AlasFx.EntityFrameworkCore.SqlServer
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
