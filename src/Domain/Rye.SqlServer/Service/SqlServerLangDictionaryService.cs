using Dapper;

using Rye.Entities.Abstractions;

using System.Collections.Generic;

namespace Rye.SqlServer.Service
{
    public class SqlServerLangDictionaryService : ILangDictionaryService
    {
        private readonly SqlServerConnectionProvider _connectionProvider;

        public SqlServerLangDictionaryService(SqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IEnumerable<IEntityLangDictionaryBase> GetEnableList()
        {
            var sql = "select lang, dicKey, dicValue from langDictionary WITH(NOLOCK)";
            using(var conn = _connectionProvider.GetReadOnlyConnection())
            {
                return conn.Query<IEntityLangDictionaryBase>(sql);
            }
        }
    }
}
