using Monica.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Monica.DataAccess;
using Monica.EntityFrameworkCore.SqlServer;
using Monica.EntityFrameworkCore.Options;
using Microsoft.Extensions.Options;
using Monica.Options;
using Monica.DataAccess.Options;

namespace Monica.Test.Domain
{
    public class WriteReadTest
    {
        private IServiceProvider _serviceProvider;

        public WriteReadTest()
        {
            var config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json")
                  .Build();
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddMonica()
                .AddDbConnections()
                .AddMonicaSqlServer()
                .AddSingleton<DbContextOptionsBuilderOptions>(new DbContextOptionsBuilderOptions(new DbContextOptionsBuilder<TestDbContext>(), null, typeof(TestDbContext)));

            _serviceProvider = services.BuildServiceProvider();

            //var dbProvider = serviceProvider.GetRequiredService<IDbProvider>();
            //var uow = dbProvider.GetUnitOfWork<TestDbContext>("TestDb"); // 访问主库

            //var repoDbTest = uow.GetRepository<DbTest, int>();
            //var obj = new DbTest { Name = "123", Date = DateTime.Now.Date };
            //repoDbTest.Insert(obj);
            //uow.SaveChanges();

            //Console.ReadKey();

            //var uow2 = dbProvider.GetUnitOfWork<TestDbContext>("TestDb_Read"); // 访问从库

            //var repoDbTest2 = uow2.GetReadOnlyRepository<DbTest, int>();
            //var data2 = repoDbTest2.GetFirstOrDefault();
            //Console.WriteLine($"id: {data2.Id} name: {data2.Name}");
            //Console.ReadKey();
        }

        private void Write(out int newId)
        {
            var dbProvider = _serviceProvider.GetRequiredService<IDbProvider>();
            var map = _serviceProvider.GetService<IOptions<DbConnectionMapOptions>>();
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("MonicaTestDb");

            var repoDbTest = uow.GetRepository<DbTest, int>();
            var obj = new DbTest { Name = "123", Date = DateTime.Now.Date };
            repoDbTest.Insert(obj);
            uow.SaveChanges();
            uow.DbContext.AsDbContext().Entry(obj);
            newId = obj.Id;
        }

        private void Read(int newId)
        {
            var dbProvider = _serviceProvider.GetRequiredService<IDbProvider>();
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("MonicaTestDb_Read");

            var repoDbTest = uow.GetReadOnlyRepository<DbTest, int>();
            var data2 = repoDbTest.GetFirstOrDefault(d => d.Id == newId);
            Assert.NotNull(data2);
            Assert.True(data2.Name == "123");
            Assert.Equal(data2.Date, DateTime.Now.Date);
            uow.SaveChanges();
        }

        [Fact]
        public void WriteAndReadTest()
        {
            Write(out int newId);
            Read(newId);
        }
    }
}
