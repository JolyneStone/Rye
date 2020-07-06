using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Raven.Configuration;
using Raven.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Raven.Options
{
    /// <summary>
    /// Raven配置选项创建者
    /// </summary>
    public class RavenOptionsSetup : IConfigureOptions<RavenOptions>
    {
        private readonly IConfiguration _configuration;

        //public RavenOptionsSetup()
        //{
        //    _configuration = ConfigurationManager.Appsettings;
        //}

        public RavenOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 配置options各属性信息
        /// </summary>
        /// <param name="options"></param>
        public void Configure(RavenOptions options)
        {
            SetDbConnectionsOptions(options);
            //SetJwtOptions(options);
            SetOAuth2Options(options);
            SetAssemblyPatterns(options);
        }

        //private void SetJwtOptions(RavenOptions options)
        //{
        //    var section = _configuration.GetSection("Raven:Jwt");
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

        private void SetOAuth2Options(RavenOptions options)
        {
            var oauth2s = new Dictionary<string, OAuth2Options>();
            options.OAuth2s = oauth2s;
            var section = _configuration.GetSection("Raven:OAuth2");
            IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
            if (dict != null)
            {
                foreach (KeyValuePair<string, OAuth2Options> item in dict)
                {
                    oauth2s.Add(item.Key, item.Value);
                }
            }
        }

        private void SetDbConnectionsOptions(RavenOptions options)
        {
            var dbConnectionMap = new Dictionary<string, DbConnectionOptions>();
            options.DbConnections = dbConnectionMap;
            IConfiguration section = _configuration.GetSection("Raven:DbConnections");
            Dictionary<string, DbConnectionOptions> dict = section.Get<Dictionary<string, DbConnectionOptions>>();
            if (dict == null || dict.Count == 0)
            {
                string connectionString = _configuration["ConnectionStrings:DefaultDbContext"];
                if (connectionString == null)
                {
                    return;
                }
                dbConnectionMap.Add("DefaultDb", new DbConnectionOptions
                {
                    ConnectionString = connectionString,
                    DatabaseType = options.DefaultDatabaseType
                });

                return;
            }

            var ambiguous = dict.Keys.GroupBy(d => d).FirstOrDefault(d => d.Count() > 1);
            if (ambiguous != null)
            {
                throw new RavenException($"数据上下文配置中存在多个配置节点拥有同一个数据库连接名称，存在二义性：{ambiguous.First()}");
            }
            foreach (var db in dict)
            {
                dbConnectionMap.Add(db.Key, db.Value);
            }
        }

        private void SetAssemblyPatterns(RavenOptions options)
        {
            var section = _configuration.GetSection("Raven:AssemblyPatterns");
            var patterns = section.Get<string[]>();
            options.AssemblyPatterns = patterns;
        }
    }
}
