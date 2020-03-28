using KiraNet.AlasFx.EntityFrameworkCore.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    public static class MySqlServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架对MySql数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFxMySql(this IServiceCollection services)
        {
            services.AddAlasFxDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, MySqlDbContextOptionsBuilderUser>();
            return services;
        }
    }
}
