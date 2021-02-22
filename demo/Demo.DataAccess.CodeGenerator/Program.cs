using Rye.CodeGenerator;
using Rye.CodeGenerator.MySql;
using Rye.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo.DataAccess.CodeGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 通过代码生成器生成访问层代码
            var connectionString = ConfigurationManager.Appsettings.GetSection("Framework:DbConnections:RyeDemo:ConnectionString").Value;
            var modelGenerator = new MySqlModelCompiler();
            await modelGenerator.GenerateAllAsync(new ModelConfig
            {
                ConnectionString = connectionString,
                Database = "RyeDemo",
                NameSpace = "Demo.DataAccess",
                FilePath = Path.Combine(@"..\..\..\..\Demo.DataAccess", "Model")
            });

            var interfaceGenerator = new MySqlInterfaceCompiler();
            await interfaceGenerator.GenerateAllAsync(new ModelConfig
            {
                ConnectionString = connectionString,
                Database = "RyeDemo",
                NameSpace = "Demo.DataAccess",
                FilePath = Path.Combine(@"..\..\..\..\Demo.DataAccess", "Interface")
            });

            var daoGenerator = new MySqlDaoCompiler();
            await daoGenerator.GenerateAllAsync(new ModelConfig
            {
                ConnectionString = connectionString,
                Database = "RyeDemo",
                NameSpace = "Demo.DataAccess",
                FilePath = Path.Combine(@"..\..\..\..\Demo.DataAccess", "Dao")
            });

            Console.WriteLine("Completed!");
            Console.ReadKey();
        }
    }
}
