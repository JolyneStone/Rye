using Microsoft.Extensions.Logging;
using Rye.Enums;
using System.Collections.Generic;

namespace Rye.Options
{
    /// <summary>
    /// Rye框架配置选项
    /// </summary>
    public class RyeOptions
    {
        public RyeOptions()
        {
            //DefaultDatabaseType = DatabaseType.SqlServer;
            //DbConnections = new Dictionary<string, DbConnectionOptions>();
            Logger = new LoggerOptions
            {
                LogPath = @"/home/admin/logs/temp",
                IsConsoleEnabled = false,
                LogLevel = LogLevel.Debug,
                UseDefaultLog = true
            };
            AssemblyPatterns = new string[0];
        }

        //public DatabaseType DefaultDatabaseType { get; set; }
        //public IDictionary<string, DbConnectionOptions> DbConnections { get; set; }

        //public IDictionary<string, OAuth2Options> OAuth2s { get; set; } = new Dictionary<string, OAuth2Options>();

        //public JwtOptions Jwt { get; set; } = new JwtOptions();

        //public RedisOptions Redis { get; set; }
        public LoggerOptions Logger { get; set; }

        public string[] AssemblyPatterns { get; set; }
    }
}
