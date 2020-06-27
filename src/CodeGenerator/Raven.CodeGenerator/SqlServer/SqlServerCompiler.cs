using Dapper;

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Data.SqlClient;

using Raven.CodeGenerator.Abstracts;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Raven.CodeGenerator.SqlServer
{
    public abstract class SqlServerCompiler : BaseCodeCompiler<ModelConfig>
    {
        public override async Task SaveAsync(ModelConfig config, ModelEntity modelEntity, Stream stream)
        {
            using (var fileStream = File.Open(Path.Combine(config.FilePath, GetFileName(modelEntity)), FileMode.Create, FileAccess.Write))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }
        }

        protected override async Task<ModelEntity> GetModelEntityAsync(ModelConfig config)
        {
            var table = await GetEntityAsync(config);
            var columns = await GetColumnsAsync(config);

            var modelEntity = new ModelEntity(table, columns);
            modelEntity.NameSpace = config.NameSpace;
            return modelEntity;
        }

        protected override RazorCodeDocument GetDocument()
        {
            var fs = RazorProjectFileSystem.Create(".");
            var razorEngine = RazorProjectEngine.Create(RazorConfiguration.Default, fs, builder =>
            {
                //InheritsDirective.Register(builder);
                builder.SetNamespace("Raven.CodeGenerator.Razor")
                    .SetBaseType($"Raven.CodeGenerator.Razor.EntityRazorPageView")
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
            using (var conn = new SqlConnection(config.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                var sql = @$"select b.name [Schema], a.name [Name], c.value [Description] from {config.Database}.sys.tables a
                            inner join sys.schemas b on a.schema_id = b.schema_id
                            left join sys.extended_properties c on c.major_id=a.object_id and c.minor_id=0 and c.class=1 
                            where b.name = @schema and a.name = @tableName";
                var parameter = new DynamicParameters();
                parameter.Add("@schema", config.Schema);
                parameter.Add("@tableName", config.Table);

                return await conn.QueryFirstOrDefaultAsync<TableFeature>(sql, parameter);
            }
        }

        private async Task<IEnumerable<ColumnFeature>> GetColumnsAsync(ModelConfig config)
        {
            using (var conn = new SqlConnection(config.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                var sql = @$"select  
                                a.Name [Name],  
                                isnull(e.[value],'') [Description],  
                                b.Name [SqlType],    
                                case when is_identity=1 then 1 else 0 end [IsIdentity],  
                                case when exists(select 1 from sys.objects x join sys.indexes y on x.Type=N'PK' and x.Name=y.Name  
                                                    join sysindexkeys z on z.ID=a.Object_id and z.indid=y.index_id and z.Colid=a.Column_id)  
                                                then 1 else 0 end [IsKey],      
                                case when a.is_nullable=1 then 1 else 0 end [IsNullable],
                                isnull(d.text,'') [DefaultValue]   
                            from Test.INFORMATION_SCHEMA.COLUMNS s
                            inner join  
                                sys.columns a on s.COLUMN_NAME COLLATE Chinese_PRC_CI_AS = a.name
                            left join 
                                sys.types b on a.user_type_id=b.user_type_id  
                            inner join 
                                sys.objects c on a.object_id=c.object_id and c.Type='U' 
                            left join 
                                syscomments d on a.default_object_id=d.ID  
                            left join
                                sys.extended_properties e on e.major_id=c.object_id and e.minor_id=a.Column_id and e.class=1   
                            where s.TABLE_SCHEMA = @schema and c.name = @tableName and s.TABLE_NAME = @tableName
                            order by a.column_id";
                var parameter = new DynamicParameters();
                parameter.Add("@schema", config.Schema);
                parameter.Add("@tableName", config.Table);

                return await conn.QueryAsync<ColumnFeature>(sql, parameter);
            }
        }
    }
}
