using Demo.Common.Enums;
using Demo.DataAccess.EFCore.DbContexts;

using Rye.EntityFrameworkCore;

namespace Demo.DataAccess.EFCore
{
    public static class DbProviderExtensions
    {
        public static IUnitOfWork GetUnitOfWorkByWriteDb(this IDbProvider provider)
        {
            return provider.GetUnitOfWork<DefaultDbContext>(DbConfig.DbRye);
        }

        public static IUnitOfWork GetUnitOfWorkByReadDb(this IDbProvider provider)
        {
            return provider.GetUnitOfWork<DefaultDbContext>(DbConfig.DbRye_Read);
        }
    }
}
