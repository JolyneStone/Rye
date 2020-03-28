using KiraNet.AlasFx.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace KiraNet.AlasFx.Options
{
    /// <summary>
    /// AlasFx配置选项创建者
    /// </summary>
    public class AlasFxOptionsSetup : IConfigureOptions<AlasFxOptions>
    {
        private readonly IConfiguration _configuration;

        public AlasFxOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 配置options各属性信息
        /// </summary>
        /// <param name="options"></param>
        public void Configure(AlasFxOptions options)
        {
            SetDbConnectionsOptions(options);
            SetJwtOptions(options);
            SetOAuth2Options(options);
            SetAssemblyPatterns(options);
        }

        private void SetJwtOptions(AlasFxOptions options)
        {
            var section = _configuration.GetSection("AlasFx:Jwt");
            JwtOptions jwt = section.Get<JwtOptions>();
            options.Jwt = jwt;
            if (jwt != null)
            {
                //if (jwt.Secret == null)
                //{
                //    jwt.Secret = section.GetValue<string>("Secret");
                //}
                var encrypt = section.GetValue<string>("Encrypt");
                if (encrypt.IsNullOrEmpty())
                {
                    jwt.Encrypt = SecurityAlgorithms.HmacSha256;
                }
            }
        }

        private void SetOAuth2Options(AlasFxOptions options)
        {
            var oauth2s = new Dictionary<string, OAuth2Options>();
            options.OAuth2s = oauth2s;
            var section = _configuration.GetSection("AlasFx:OAuth2");
            IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
            if (dict != null)
            {
                foreach (KeyValuePair<string, OAuth2Options> item in dict)
                {
                    oauth2s.Add(item.Key, item.Value);
                }
            }
        }

        private void SetDbConnectionsOptions(AlasFxOptions options)
        {
            var dbConnectionMap = new Dictionary<string, DbConnectionOptions>();
            options.DbConnections = dbConnectionMap;
            IConfiguration section = _configuration.GetSection("AlasFx:DbConnections");
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
                throw new AlasFxException($"数据上下文配置中存在多个配置节点拥有同一个数据库连接名称，存在二义性：{ambiguous.First()}");
            }
            foreach (var db in dict)
            {
                dbConnectionMap.Add(db.Key, db.Value);
            }
        }

        private void SetAssemblyPatterns(AlasFxOptions options)
        {
            var section = _configuration.GetSection("AlasFx:AssemblyPatterns");
            var patterns = section.Get<string[]>();
            options.AssemblyPatterns = patterns;
        }
    }
}
