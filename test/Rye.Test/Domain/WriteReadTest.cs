using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.DataAccess.Options;
using Rye.EntityFrameworkCore;
using Rye.EntityFrameworkCore.Options;

using System;

using Xunit;

namespace Rye.Test.Domain
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
                .AddRye()
                .AddDbConnections()
                .AddRyeSqlServer()
                .AddSingleton<DbContextOptionsBuilderOptions<TestDbContext>>(new DbContextOptionsBuilderOptions<TestDbContext>(new DbContextOptionsBuilder<TestDbContext>(), null));

            _serviceProvider = services.BuildServiceProvider();

            var dbProvider = _serviceProvider.GetRequiredService<IDbProvider>();
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("RyeTestDb"); // 访问主库

            var repoDbTest = uow.GetRepository<DbTest, int>();
            var obj = new DbTest { Name = "123", Date = DateTime.Now.Date };
            repoDbTest.Insert(obj);
            uow.SaveChanges();

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
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("RyeTestDb");

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
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("RyeTestDb_Read");

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
