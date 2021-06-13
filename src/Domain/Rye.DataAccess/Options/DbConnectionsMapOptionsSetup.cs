using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Rye.Exceptions;

using System.Linq;

namespace Rye.DataAccess.Options
{
    /// <summary>
    /// Rye配置选项创建者
    /// </summary>
    public class DbConnectionsMapOptionsSetup : IConfigureOptions<DbConnectionMapOptions>
    {
        private readonly IConfigurationSection _configurationSection;

        //public RyeOptionsSetup()
        //{
        //    _configuration = App.Configuration;
        //}

        public DbConnectionsMapOptionsSetup(IConfiguration configuration)
        {
            _configurationSection = configuration.GetSection("Framework");
        }

        internal DbConnectionsMapOptionsSetup(IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;
        }

        /// <summary>
        /// 配置options各属性信息
        /// </summary>
        /// <param name="options"></param>
        public void Configure(DbConnectionMapOptions options)
        {
            //SetDefaultDatabaseType(options);
            SetDbConnectionsOptions(options);
        }

        //private void SetDefaultDatabaseType(DbConnectionMapOptions options)
        //{
        //    var defaultDatabaseType = _configurationSection.GetValue<DatabaseType?>("DefaultDatabaseType");
        //    options.DefaultDatabaseType = defaultDatabaseType?? DatabaseType.SqlServer;
        //}

        private void SetDbConnectionsOptions(DbConnectionMapOptions options)
        {
            IConfiguration section = _configurationSection.GetSection("DbConnections");
            var dict = section.Get<DbConnectionMapOptions>();
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
                throw new RyeException($"数据上下文配置中存在多个配置节点拥有同一个数据库连接名称，存在二义性：{ambiguous.First()}");
            }

            options = dict;

            if (options == null)
            {
                options = new DbConnectionMapOptions();
            }
        }
    }
}
