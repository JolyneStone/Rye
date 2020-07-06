using Raven.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Raven.EntityFrameworkCore
{
    /// <summary>
    /// 配置DbContextOptionsBuilder的数据库连接
    /// </summary>
    public interface IDbContextConnectionConfigure
    {
        DbContextOptionsBuilder Configure(DbContextOptionsBuilder builder);
    }
}