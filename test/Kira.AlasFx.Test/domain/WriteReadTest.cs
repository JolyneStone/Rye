using Kira.AlasFx.Core;
using Kira.AlasFx.Domain;
using Kira.AlasFx.EntityFrameworkCore;
using Kira.AlasFx.Test.domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Kira.AlasFx.Test
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
                .AddSingleton<IConfigurationRoot>(config)
                .AddSingleton<IConfiguration>(config)
                .AddAlasFx()
                .AddAlasFxDatabase()
                .AddAlasFxSqlServer()
                .AddDbBuilderOptions<TestDbContext>(null, builder =>
                {

                });

            _serviceProvider = services.BuildServiceProvider();
        }

        private void Write(out int newId)
        {
            var dbProvider = _serviceProvider.GetRequiredService<IDbProvider>();
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("AlasTestDb");
            
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
            var uow = dbProvider.GetUnitOfWork<TestDbContext>("AlasTestDb_Read");

            var repoDbTest = uow.GetReadOnlyRepository<DbTest, int>();
            var data2 = repoDbTest.GetFirstOrDefault(d=>d.Id == newId);
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
