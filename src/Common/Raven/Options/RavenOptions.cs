using Raven.Enums;
using System.Collections.Generic;

namespace Raven.Options
{
    /// <summary>
    /// Raven框架配置选项
    /// </summary>
    public class RavenOptions
    {
        public RavenOptions()
        {
        }

        public DatabaseType DefaultDatabaseType { get; set; } = DatabaseType.SqlServer;
        public IDictionary<string, DbConnectionOptions> DbConnections { get; set; } = new Dictionary<string, DbConnectionOptions>();

        public IDictionary<string, OAuth2Options> OAuth2s { get; set; } = new Dictionary<string, OAuth2Options>();

        //public JwtOptions Jwt { get; set; } = new JwtOptions();

        //public RedisOptions Redis { get; set; }
        public string[] AssemblyPatterns { get; set; }
    }
}
