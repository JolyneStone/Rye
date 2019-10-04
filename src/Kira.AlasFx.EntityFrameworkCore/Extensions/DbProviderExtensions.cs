using Kira.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kira.AlasFx.EntityFrameworkCore
{
    public static class DbProviderExtensions
    {
        /// <summary>
        /// 根据上下文类型及数据库名称获取UnitOfWork对象, dbName为null时默认为第一个数据库名称
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="provider"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static IUnitOfWork GetUnitOfWork<TDbContext>(this IDbProvider provider, string dbName = null)
            where TDbContext : DbContext, IDbContext
        {
            Check.NotNull(provider, nameof(provider));
            return provider.GetUnitOfWork(typeof(TDbContext), dbName);
        }
    }
}
