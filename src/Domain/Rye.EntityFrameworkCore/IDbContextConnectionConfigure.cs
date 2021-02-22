using Rye.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Rye.EntityFrameworkCore
{
    /// <summary>
    /// 配置DbContextOptionsBuilder的数据库连接
    /// </summary>
    public interface IDbContextConnectionConfigure
    {
        DbContextOptionsBuilder Configure(DbContextOptionsBuilder builder);
    }
}