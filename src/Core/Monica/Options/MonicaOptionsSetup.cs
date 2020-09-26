using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Monica.Configuration;
using Monica.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Monica.Options
{
    /// <summary>
    /// Monica配置选项创建者
    /// </summary>
    public class MonicaOptionsSetup : IConfigureOptions<MonicaOptions>
    {
        private readonly IConfigurationSection _configurationSection;

        //public MonicaOptionsSetup()
        //{
        //    _configuration = ConfigurationManager.Appsettings;
        //}

        public MonicaOptionsSetup(IConfiguration configuration)
        {
            _configurationSection = configuration.GetSection("Framework");
        }

        internal MonicaOptionsSetup(IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;
        }

        /// <summary>
        /// 配置options各属性信息
        /// </summary>
        /// <param name="options"></param>
        public void Configure(MonicaOptions options)
        {
            SetDbConnectionsOptions(options);
            //SetJwtOptions(options);
            //SetOAuth2Options(options);
            SetAssemblyPatterns(options);
            SetLoggerOptions(options);
        }

        //private void SetJwtOptions(MonicaOptions options)
        //{
        //    var section = _configuration.GetSection("Framework:Jwt");
        //    JwtOptions jwt = section.Get<JwtOptions>();
        //    options.Jwt = jwt;
        //    if (jwt != null)
        //    {
        //        //if (jwt.Secret == null)
        //        //{
        //        //    jwt.Secret = section.GetValue<string>("Secret");
        //        //}
        //        var encrypt = section.GetValue<string>("Encrypt");
        //        if (encrypt.IsNullOrEmpty())
        //        {
        //            jwt.Encrypt = SecurityAlgorithms.HmacSha256;
        //        }
        //    }
        //}

        //private void SetOAuth2Options(MonicaOptions options)
        //{
        //    var oauth2s = new Dictionary<string, OAuth2Options>();
        //    options.OAuth2s = oauth2s;
        //    var section = _configuration.GetSection("Framework:OAuth2");
        //    IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
        //    if (dict != null)
        //    {
        //        foreach (KeyValuePair<string, OAuth2Options> item in dict)
        //        {
        //            oauth2s.Add(item.Key, item.Value);
        //        }
        //    }
        //}

        private void SetDbConnectionsOptions(MonicaOptions options)
        {
            var dbConnectionMap = new Dictionary<string, DbConnectionOptions>();
            options.DbConnections = dbConnectionMap;
            IConfiguration section = _configurationSection.GetSection("DbConnections");
            Dictionary<string, DbConnectionOptions> dict = section.Get<Dictionary<string, DbConnectionOptions>>();
            if (dict == null || dict.Count == 0)
            {
                //string connectionString = _configurationSection["DefaultDbContext"];
                //if (connectionString == null)
                //{
                //    return;
                //}
                //dbConnectionMap.Add("DefaultDb", new DbConnectionOptions
                //{
                //    ConnectionString = connectionString,
                //    DatabaseType = options.DefaultDatabaseType
                //});

                return;
            }

            var ambiguous = dict.Keys.GroupBy(d => d).FirstOrDefault(d => d.Count() > 1);
            if (ambiguous != null)
            {
                throw new MonicaException($"数据上下文配置中存在多个配置节点拥有同一个数据库连接名称，存在二义性：{ambiguous.First()}");
            }
            foreach (var db in dict)
            {
                dbConnectionMap.Add(db.Key, db.Value);
            }
        }

        private void SetAssemblyPatterns(MonicaOptions options)
        {
            var section = _configurationSection.GetSection("AssemblyPatterns");
            var patterns = section.Get<string[]>();
            options.AssemblyPatterns = patterns;
        }

        private void SetLoggerOptions(MonicaOptions options)
        {
            var section = _configurationSection.GetSection("Logger");
            options.Logger = section.Get<LoggerOptions>();
        }
    }
}
