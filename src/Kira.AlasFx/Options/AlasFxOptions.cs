using Kira.AlasFx.Domain;
using System.Collections.Generic;

namespace Kira.AlasFx.Options
{
    /// <summary>
    /// Kira.AlasFx框架配置选项
    /// </summary>
    public class AlasFxOptions
    {
        public AlasFxOptions()
        {
        }

        public DatabaseType DefaultDatabaseType { get; set; } = DatabaseType.SqlServer;
        public IDictionary<string, DbConnectionOptions> DbConnections { get; set; }

        public IDictionary<string, OAuth2Options> OAuth2s { get; set; }

        public JwtOptions Jwt { get; set; }

        public RedisOptions Redis { get; set; }
    }
}
