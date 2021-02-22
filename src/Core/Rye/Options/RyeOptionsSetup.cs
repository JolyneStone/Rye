//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Rye.Configuration;
//using Rye.Enums;
//using Rye.Exceptions;
//using System.Collections.Generic;
//using System.Linq;

//namespace Rye.Options
//{
//    /// <summary>
//    /// Rye配置选项创建者
//    /// </summary>
//    public class RyeOptionsSetup : IConfigureOptions<RyeOptions>
//    {
//        private readonly IConfigurationSection _configurationSection;

//        //public RyeOptionsSetup()
//        //{
//        //    _configuration = ConfigurationManager.Appsettings;
//        //}

//        public RyeOptionsSetup(IConfiguration configuration)
//        {
//            _configurationSection = configuration.GetSection("Framework");
//        }

//        internal RyeOptionsSetup(IConfigurationSection configurationSection)
//        {
//            _configurationSection = configurationSection;
//        }

//        /// <summary>
//        /// 配置options各属性信息
//        /// </summary>
//        /// <param name="options"></param>
//        public void Configure(RyeOptions options)
//        {
//            SetDefaultDatabaseType(options);
//            SetDbConnectionsOptions(options);
//            //SetJwtOptions(options);
//            //SetOAuth2Options(options);
//            SetAssemblyPatterns(options);
//            SetLoggerOptions(options);
//        }

//        //private void SetJwtOptions(RyeOptions options)
//        //{
//        //    var section = _configuration.GetSection("Framework:Jwt");
//        //    JwtOptions jwt = section.Get<JwtOptions>();
//        //    options.Jwt = jwt;
//        //    if (jwt != null)
//        //    {
//        //        //if (jwt.Secret == null)
//        //        //{
//        //        //    jwt.Secret = section.GetValue<string>("Secret");
//        //        //}
//        //        var encrypt = section.GetValue<string>("Encrypt");
//        //        if (encrypt.IsNullOrEmpty())
//        //        {
//        //            jwt.Encrypt = SecurityAlgorithms.HmacSha256;
//        //        }
//        //    }
//        //}

//        //private void SetOAuth2Options(RyeOptions options)
//        //{
//        //    var oauth2s = new Dictionary<string, OAuth2Options>();
//        //    options.OAuth2s = oauth2s;
//        //    var section = _configuration.GetSection("Framework:OAuth2");
//        //    IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
//        //    if (dict != null)
//        //    {
//        //        foreach (KeyValuePair<string, OAuth2Options> item in dict)
//        //        {
//        //            oauth2s.Add(item.Key, item.Value);
//        //        }
//        //    }
//        //}

//        private void SetDefaultDatabaseType(RyeOptions options)
//        {
//            var defaultDatabaseType = _configurationSection.GetValue<DatabaseType?>("DefaultDatabaseType");
//            options.DefaultDatabaseType = defaultDatabaseType?? DatabaseType.SqlServer;
//        }

//        private void SetDbConnectionsOptions(RyeOptions options)
//        {
//            var dbConnectionMap = new Dictionary<string, DbConnectionOptions>();
//            options.DbConnections = dbConnectionMap;
//            IConfiguration section = _configurationSection.GetSection("DbConnections");
//            Dictionary<string, DbConnectionOptions> dict = section.Get<Dictionary<string, DbConnectionOptions>>();
//            if (dict == null || dict.Count == 0)
//            {
//                //string connectionString = _configurationSection["DefaultDbContext"];
//                //if (connectionString == null)
//                //{
//                //    return;
//                //}
//                //dbConnectionMap.Add("DefaultDb", new DbConnectionOptions
//                //{
//                //    ConnectionString = connectionString,
//                //    DatabaseType = options.DefaultDatabaseType
//                //});

//                return;
//            }

//            var ambiguous = dict.Keys.GroupBy(d => d).FirstOrDefault(d => d.Count() > 1);
//            if (ambiguous != null)
//            {
//                throw new RyeException($"数据上下文配置中存在多个配置节点拥有同一个数据库连接名称，存在二义性：{ambiguous.First()}");
//            }
//            foreach (var db in dict)
//            {
//                dbConnectionMap.Add(db.Key, db.Value);
//            }

//            if (options.DbConnections == null)
//            {
//                options.DbConnections = new Dictionary<string, DbConnectionOptions>();
//            }
//        }

//        private void SetAssemblyPatterns(RyeOptions options)
//        {
//            var section = _configurationSection.GetSection("AssemblyPatterns");
//            var patterns = section.Get<string[]>();
//            options.AssemblyPatterns = patterns;
//            if(options.AssemblyPatterns == null)
//            {
//                options.AssemblyPatterns = new string[0];
//            }
//        }

//        private void SetLoggerOptions(RyeOptions options)
//        {
//            var section = _configurationSection.GetSection("Logger");
//            options.Logger = section.Get<LoggerOptions>();
//            if (options.Logger == null)
//            {
//                options.Logger = new LoggerOptions
//                {
//                    LogPath = @"/home/admin/logs/temp",
//                    IsConsoleEnabled = false,
//                    LogLevel = LogLevel.Debug,
//                    UseRyeLog = true
//                };
//            }
//        }
//    }
//}
