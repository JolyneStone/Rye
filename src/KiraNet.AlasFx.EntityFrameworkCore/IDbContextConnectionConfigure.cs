using KiraNet.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    /// <summary>
    /// 配置DbContextOptionsBuilder的数据库连接
    /// </summary>
    public interface IDbContextConnectionConfigure
    {
        DbContextOptionsBuilder Configure(DbContextOptionsBuilder builder);
    }
}