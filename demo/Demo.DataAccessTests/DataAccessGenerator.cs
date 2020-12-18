using Monica.CodeGenerator;
using Monica.CodeGenerator.MySql;
using Monica.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace Demo.DataAccess.Tests
{
    public class DataAccessGenerator
    {
        private static readonly string ConnectionString = ConfigurationManager.GetSectionValue($"Framework:DbConnections:MonicaDemo:ConnectionString");

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
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = ConnectionString,
                Database = "MonicaDemo",
                Table = "userInfo",
                NameSpace = "Demo.DataAccess",
                FilePath = @"C:\monster\Code\KiraNet\Monica\demo\Demo.DataAccess\Model"
            });
        }

        [Fact()]
        public async Task GenerateInterfaceAsync()
        {
            var generator = new MySqlInterfaceCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = ConnectionString,
                Database = "MonicaDemo",
                Table = "userInfo",
                NameSpace = "Demo.DataAccess",
                FilePath = @"C:\monster\Code\KiraNet\Monica\demo\Demo.DataAccess\Interface"
            });
        }

        [Fact()]
        public async Task GenerateDaoAsync()
        {
            var generator = new MySqlDaoCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = ConnectionString,
                Database = "MonicaDemo",
                Table = "userInfo",
                NameSpace = "Demo.DataAccess",
                FilePath = @"C:\monster\Code\KiraNet\Monica\demo\Demo.DataAccess\Dao"
            });
        }
    }
}