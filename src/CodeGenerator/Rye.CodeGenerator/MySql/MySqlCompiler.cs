using Dapper;

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;

using MySql.Data.MySqlClient;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rye.CodeGenerator.MySql
{
    public abstract class MySqlCompiler : BaseDbCodeCompiler<ModelConfig>
    {
        public override async Task SaveAsync(ModelConfig config, ModelEntity modelEntity, Stream stream)
        {
            using (var fileStream = File.Open(Path.Combine(config.FilePath, GetFileName(modelEntity)), FileMode.Create, FileAccess.Write))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }
        }

        public virtual async Task GenerateAllAsync(DbCodeConfig config)
        {
            var list = await GetAllTable(config);
            if (list == null)
                return;
            foreach (var (Schema, Table) in list)
            {
                var modelConfig = new ModelConfig
                {
                    Schema = Schema,
                    Table = Table,
                    ConnectionString = config.ConnectionString,
                    NameSpace = config.NameSpace,
                    Database = config.Database,
                    FilePath = config.FilePath
                };

                await GenerateAsync(modelConfig);
            }
        }

        protected async Task<IEnumerable<ModelEntity>> GetAllModelEntityAsync(DbCodeConfig config)
        {
            var list = await GetAllTable(config);
            if (list == null)
                return default;
            var entityList = new List<ModelEntity>();
            foreach (var (Schema, Table) in list)
            {
                var modelConfig = new ModelConfig
                {
                    Schema = Schema,
                    Table = Table,
                    ConnectionString = config.ConnectionString,
                    NameSpace = config.NameSpace,
                    Database = config.Database,
                    FilePath = config.FilePath
                };
                entityList.Add(await GetModelEntityAsync(modelConfig));
            }

            return entityList;
        }

        protected override async Task<ModelEntity> GetModelEntityAsync(ModelConfig config)
        {
            var table = await GetEntityAsync(config);
            var columns = await GetColumnsAsync(config);

            var modelEntity = new ModelEntity(table, columns)
            {
                NameSpace = config.NameSpace
            };
            return modelEntity;
        }

        protected override RazorCodeDocument GetDocument()
        {
            var fs = RazorProjectFileSystem.Create(".");
            var razorEngine = RazorProjectEngine.Create(RazorConfiguration.Default, fs, builder =>
            {
                //InheritsDirective.Register(builder);
                builder.SetNamespace("Rye.CodeGenerator.Razor")
                    .SetBaseType($"Rye.CodeGenerator.MySql.MySqlRazorPageView")
                    .SetCSharpLanguageVersion(LanguageVersion.Default)
                    .AddDefaultImports(new string[]
                    {
                        "using System",
                        "using System.Threading.Tasks",
                        "using System.Collections.Generic",
                        "using System.Collections"
                    })
                    .ConfigureClass((document, node) =>
                    {
                        node.ClassName = "TempleteRazorPageView";
                    });
            });

            var path = Path.Combine("Templates", GetTemplate());
            var template = fs.GetItem(path, FileKinds.GetFileKindFromFilePath(path));

            return razorEngine.Process(template);
        }

        protected abstract string GetTemplate();

        protected abstract string GetFileName(ModelEntity entity);

        private async Task<TableFeature> GetEntityAsync(ModelConfig config)
        {
            using (var conn = new MySqlConnection(config.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                var sql = @$"select ENGINE AS 'Schema', TABLE_NAME AS 'Name', TABLE_COMMENT AS 'Description' from INFORMATION_SCHEMA.TABLES
                                where TABLE_SCHEMA = @database and TABLE_NAME = @tableName LIMIT 1";
                var parameter = new DynamicParameters();
                parameter.Add("@database", config.Database);
                parameter.Add("@tableName", config.Table);

                return await conn.QueryFirstOrDefaultAsync<TableFeature>(sql, parameter);
            }
        }

        private async Task<IEnumerable<(string Schema, string Table)>> GetAllTable(DbCodeConfig config)
        {
            using (var conn = new MySqlConnection(config.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                var sql = @$"select ENGINE AS 'Schema', TABLE_NAME AS 'Name', TABLE_COMMENT AS 'Description' from INFORMATION_SCHEMA.TABLES
                                where TABLE_SCHEMA = @database";
                var parameter = new DynamicParameters();
                parameter.Add("@database", config.Database);
                return await conn.QueryAsync<(string, string)>(sql, parameter);
            }
        }

        private async Task<IEnumerable<ColumnFeature>> GetColumnsAsync(ModelConfig config)
        {
            using (var conn = new MySqlConnection(config.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                var sql = @$"SELECT
                                Column_Name AS 'Name',
		                            COLUMN_COMMENT AS 'Description',
                                DATA_TYPE AS 'SqlType',
                                (
                                    CASE WHEN EXTRA = 'auto_increment' THEN 1 ELSE 0 END
                                ) AS 'IsIdentity',
                                (
                                    CASE WHEN COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END
                                ) AS 'IsKey',
                                (
                                    CASE WHEN IS_NULLABLE = 'NO' THEN 0 ELSE 1 END
                                ) AS 'IsNullable',
                                COLUMN_DEFAULT AS 'DefaultValue'
                            FROM
                                INFORMATION_SCHEMA.COLUMNS
		                            where table_schema =@database and table_name = @tableName
		                            order by ORDINAL_POSITION";
                var parameter = new DynamicParameters();
                parameter.Add("@database", config.Database);
                parameter.Add("@tableName", config.Table);

                return await conn.QueryAsync<ColumnFeature>(sql, parameter);
            }
        }
    }
}
