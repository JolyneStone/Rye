using Dapper;

using Microsoft.Extensions.Localization;

using Rye;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Demo.Localization.StringLocalizer
{
    /// <summary>
    /// 以数据库作为本地化资源的存储方案，实现从数据库中读取本地化资源
    /// </summary>
    public class SqlStringLocalizer : IStringLocalizer
    {
        private readonly SqlStringLocalizerOptions _options;

        public SqlStringLocalizer(SqlStringLocalizerOptions options)
        {
            _options = options;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var val = GetString(name);
                return new LocalizedString(name, val, resourceNotFound: val.IsNullOrEmpty());
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var val = GetString(name);
                if (!val.IsNullOrEmpty())
                {
                    val = string.Format(val, arguments);
                }
                return new LocalizedString(name, val, resourceNotFound: val.IsNullOrEmpty());
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var culture = CultureInfo.CurrentCulture.ToString();
            using (var conn = _options.CreateDbConnection())
            {
                conn.Open();
                var sql = "select `dicKey`, `dicValue` from `langDictionary` where lang = @lang";
                var parameters = new DynamicParameters();
                parameters.Add("@lang", culture);
                var data = conn.Query<(string dicKey, string dicValue)>(sql, param: parameters);
                return data.Select(d => new LocalizedString(d.dicKey, d.dicValue));
            }
        }

        private string GetString(string key)
        {
            var culture = CultureInfo.CurrentCulture.ToString();
            using (var conn = _options.CreateDbConnection())
            {
                conn.Open();
                var sql = @"select top 1 `dicValue` from `langDictionary` where lang = @lang and dicKey = @key";
                var parameters = new DynamicParameters();
                parameters.Add("@lang", culture);
                parameters.Add("@key", key);

                return conn.QueryFirstOrDefault(sql, param: parameters);
            }
        }
    }
}
