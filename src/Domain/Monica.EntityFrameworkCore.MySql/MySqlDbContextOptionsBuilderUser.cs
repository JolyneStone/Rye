using Microsoft.EntityFrameworkCore;

using Monica.Enums;

namespace Monica.EntityFrameworkCore.MySql
{
    public class MySqlDbContextOptionsBuilderUser : IDbContextOptionsBuilderUser
    {
        public DatabaseType Type => DatabaseType.MySql;

        public DbContextOptionsBuilder<TDbContext> Use<TDbContext>(DbContextOptionsBuilder<TDbContext> builder, string connectionString)
            where TDbContext : DbContext, IDbContext
        {
            return builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
