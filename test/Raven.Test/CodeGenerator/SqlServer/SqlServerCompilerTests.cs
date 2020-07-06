using Xunit;
using Raven.CodeGenerator.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Raven.CodeGenerator.CodeGenerator.SqlServer.Tests
{
    public class SqlServerCompilerTests
    {
        [Fact()]
        public async Task GenerateModelAsyncTest()
        {
            var generator = new SqlServerModelCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "Persist Security Info=False;User ID=sa;Password=sqlzzq;Initial Catalog=test;Data Source=localhost;",
                Database = "test",
                Schema = "dbo",
                Table = "Test1",
                NameSpace = "Raven.DataAccess.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }

        [Fact()]
        public async Task GenerateInterfaceAsyncTest()
        {
            var generator = new SqlServerInterfaceCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "Persist Security Info=False;User ID=sa;Password=sqlzzq;Initial Catalog=test;Data Source=localhost;",
                Database = "test",
                Schema = "dbo",
                Table = "Test1",
                NameSpace = "Raven.DataAccess.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }
    }
}