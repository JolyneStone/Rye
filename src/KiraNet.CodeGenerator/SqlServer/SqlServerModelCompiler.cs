using KiraNet.CodeGenerator.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;

namespace KiraNet.CodeGenerator.SqlServer
{
    public class SqlServerModelCompiler : ICodeCompiler<ModelConfig>
    {
        public async Task<RazorPageGeneratorResult> GenerateAsync(ModelConfig config)
        {
            var table = await GetEntityAsync(config);

        }

        private async Task<TableStructure> GetEntityAsync(ModelConfig config)
        {
            using(var conn = new SqlConnection(config.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                var sql = @$"select b.name Scheam, a.name Name, c.value Description from {config.Database}.sys.tables a
                            inner join sys.schemas b on a.schema_id = b.schema_id
                            left join sys.extended_properties c on c.major_id=a.object_id and c.minor_id=0 and c.class=1 
                            where b.name = @schema and a.name = @tableName";
                var parameter = new DynamicParameters();
                parameter.Add("@schema", config.Schema);
                parameter.Add("@tableName", config.Table);

                return await conn.QueryFirstOrDefaultAsync<TableStructure>(sql, parameter);
            }
        }
    }
}