using Monica.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Monica.EntityFrameworkCore
{
    /// <summary>
    /// 配置DbContextOptionsBuilder的数据库连接
    /// </summary>
    public interface IDbContextConnectionConfigure
    {
        DbContextOptionsBuilder Configure(DbContextOptionsBuilder builder);
    }
}