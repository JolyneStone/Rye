using Monica.DataAccess;
using System;

namespace Monica.EntityFrameworkCore
{
    public static class DbProviderExtension
    {
        /// <summary>
        /// 根据上下文类型及数据库名称获取UnitOfWork对象
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static IUnitOfWork GetUnitOfWork(this IDbProvider provider, Type dbContextType, Enum @enum)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(dbContextType, nameof(dbContextType));
            Check.NotNull(@enum, nameof(@enum));

            var dbName = @enum.GetDescription();
            return provider.GetUnitOfWork(dbContextType, dbName);
        }

        /// <summary>
        /// 根据上下文类型及数据库名称获取UnitOfWork对象
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static IUnitOfWork GetUnitOfWork<TDbContext>(this IDbProvider provider, Enum @enum)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(@enum, nameof(@enum));

            var dbName = @enum.GetDescription();
            return provider.GetUnitOfWork<TDbContext>(dbName);
        }
    }
}
