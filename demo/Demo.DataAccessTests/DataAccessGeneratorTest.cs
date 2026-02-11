using Microsoft.SqlServer.Server;
using Rye;
using Rye.CodeGenerator;
using Rye.CodeGenerator.MySql;

using System.Threading.Tasks;
using Xunit;


namespace Demo.DataAccess.Tests
{
    public class DataAccessGeneratorTest
    {
        private static readonly string ConnectionString = App.Configuration.GetSectionValue($"Framework:DbConnections:RyeDemo:ConnectionString");

        [Fact()]
        public async Task GenerateAsync()
        {
            await GenerateModelAsync();
            await GenerateInterfaceAsync();
            await GenerateDaoAsync();
        }

        [Fact()]
        public async Task GenerateModelAsync()
        {
            var generator = new MySqlModelCompiler();
            await generator.GenerateAllAsync(new ModelConfig
            {
                ConnectionString = ConnectionString,
                Database = "RyeDemo",
                //Table = "userInfo",
                NameSpace = "Demo.DataAccess",
                FilePath = @"C:\monster\Code\KiraNet\Rye\demo\Demo.DataAccess\Model"
            });
        }

        [Fact()]
        public async Task GenerateInterfaceAsync()
        {
            var generator = new MySqlInterfaceCompiler();
            await generator.GenerateAllAsync(new ModelConfig
            {
                ConnectionString = ConnectionString,
                Database = "RyeDemo",
                //Table = "userInfo",
                NameSpace = "Demo.DataAccess",
                FilePath = @"C:\monster\Code\KiraNet\Rye\demo\Demo.DataAccess\Interface"
            });
        }

        [Fact()]
        public async Task GenerateDaoAsync()
        {
            var generator = new MySqlDaoCompiler();
            await generator.GenerateAllAsync(new ModelConfig
            {
                ConnectionString = ConnectionString,
                Database = "RyeDemo",
                //Table = "userInfo",
                NameSpace = "Demo.DataAccess",
                FilePath = @"C:\monster\Code\KiraNet\Rye\demo\Demo.DataAccess\Dao"
            });
        }
    }
}