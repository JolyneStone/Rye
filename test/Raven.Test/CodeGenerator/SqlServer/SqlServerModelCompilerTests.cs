using Xunit;
using Raven.CodeGenerator.SqlServer;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Raven.CodeGenerator.CodeGenerator.SqlServer.Tests
{
    public class SqlServerModelCompilerTests
    {
        [Fact()]
        public async Task GenerateAsyncTest()
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

            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "Persist Security Info=False;User ID=sa;Password=sqlzzq;Initial Catalog=test;Data Source=localhost;",
                Database = "test",
                Schema = "dbo",
                Table = "Test2",
                NameSpace = "Raven.DataAccess.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }
    }
}