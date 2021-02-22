using Rye.CodeGenerator;
using Rye.CodeGenerator.MySql;

using System;
using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace Rye.Test.CodeGenerator.MySql
{
    public class MySqlCompilerTests
    {
        [Fact()]
        public async Task GenerateModelAsyncTest()
        {
            var generator = new MySqlModelCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "server=127.0.0.1;database=test;uid=root;pwd=Mysql_zzq123;pooling=false;SslMode=None;CharSet=utf8mb4;port=3306",
                Database = "test",
                Table = "ExternNews",
                NameSpace = "Rye.DataAccess.MySql.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }

        [Fact()]
        public async Task GenerateInterfaceAsyncTest()
        {
            var generator = new MySqlInterfaceCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "server=127.0.0.1;database=test;uid=root;pwd=Mysql_zzq123;pooling=false;SslMode=None;CharSet=utf8mb4;port=3306",
                Database = "test",
                Table = "ExternNews",
                NameSpace = "Rye.DataAccess.MySql.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }

        [Fact()]
        public async Task GenerateDaoAsyncTest()
        {
            var generator = new MySqlDaoCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "server=127.0.0.1;database=test;uid=root;pwd=Mysql_zzq123;pooling=false;SslMode=None;CharSet=utf8mb4;port=3306",
                Database = "test",
                Table = "ExternNews",
                NameSpace = "Rye.DataAccess.MySql.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }

    }
}
