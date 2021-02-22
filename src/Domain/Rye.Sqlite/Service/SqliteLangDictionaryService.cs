using Dapper;

using Rye.Entities.Abstractions;

using System.Collections.Generic;

namespace Rye.Sqlite.Service
{
    public class SqliteLangDictionaryService : ILangDictionaryService
    {
        private readonly SqliteConnectionProvider _connectionProvider;

        public SqliteLangDictionaryService(SqliteConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IEnumerable<IEntityLangDictionaryBase> GetEnableList()
        {
            var sql = "select lang, dicKey, dicValue from langDictionary";
            using(var conn = _connectionProvider.GetReadOnlyConnection())
            {
                return conn.Query<IEntityLangDictionaryBase>(sql);
            }
        }
    }
}
