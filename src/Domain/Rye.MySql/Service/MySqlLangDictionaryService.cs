using Dapper;
using Rye.Entities.Abstractions;
using System.Collections.Generic;

namespace Rye.MySql.Service
{
    public class MySqlLangDictionaryService : ILangDictionaryService
    {
        private readonly MySqlConnectionProvider _connectionProvider;

        public MySqlLangDictionaryService(MySqlConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IEnumerable<IEntityLangDictionaryBase> GetEnableList()
        {
            var sql = "select `lang`, `dicKey`, `dicValue` from `langDictionary`";
            using(var conn = _connectionProvider.GetReadOnlyConnection())
            {
                return conn.Query<IEntityLangDictionaryBase>(sql);
            }
        }
    }
}
