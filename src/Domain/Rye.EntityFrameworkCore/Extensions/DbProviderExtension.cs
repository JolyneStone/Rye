using Microsoft.EntityFrameworkCore;
using System;

namespace Rye.EntityFrameworkCore
{
    public static class DbProviderExtension
    {
        /// <summary>
        /// 根据上下文类型及数据库名称获取UnitOfWork对象
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static IUnitOfWork GetUnitOfWork<TDbContext>(this IDbProvider provider, Enum @enum)
            where TDbContext: DbContext, IDbContext
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(@enum, nameof(@enum));

            var dbName = @enum.GetDescription();
            return provider.GetUnitOfWork<TDbContext>(dbName);
        }
    }
}
