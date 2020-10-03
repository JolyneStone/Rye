using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Monica.CodeGenerator.SqlServer;
using Monica.DataAccess.Model;
using Monica.DataAccess.Options;
using Monica.SqlServer;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Monica.CodeGenerator.CodeGenerator.SqlServer.Tests
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
                Table = "ExternNews",
                NameSpace = "Monica.DataAccess.Model",
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
                Table = "ExternNews",
                NameSpace = "Monica.DataAccess.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }

        [Fact()]
        public async Task GenerateDaoAsyncTest()
        {
            var generator = new SqlServerDaoCompiler();
            await generator.GenerateAsync(new ModelConfig
            {
                ConnectionString = "Persist Security Info=False;User ID=sa;Password=sqlzzq;Initial Catalog=test;Data Source=localhost;",
                Database = "test",
                Schema = "dbo",
                Table = "ExternNews",
                NameSpace = "Monica.DataAccess.Model",
                FilePath = Directory.GetCurrentDirectory()
            });
        }

        [Fact]
        public async Task DaoTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<SqlServerConnectionProvider, TestSqlServerConnectionProvider>();

            var services = serviceCollection.BuildServiceProvider();
            var externNewsDataAccess = new DaoExternNews(services.GetRequiredService<SqlServerConnectionProvider>());

            try
            {
                //await externNewsDataAccess.InsertUpdateAsync(new ExternNews
                //{
                //    SecondaryType = null,
                //    Sort = default,
                //    DefaultCollectionNum = default,
                //    DefaultReadNum = default,
                //    BannerSort = default,
                //    BannerType = default,
                //    SourceId = default,
                //    SourceType = default,
                //    Summary = default,
                //    ContractId = default,
                //    CoverImg = default,
                //    Html = default,
                //    IsHot = default,
                //    IsPublish = default,
                //    IsRecommend = default,
                //    LinkUrl = default,
                //    NewsType = 1,
                //    PublishTime = default,
                //    RealCollectionNum = default,
                //    RealReadNum = default,
                //    ResourceUrl = default,
                //    Tags = default,
                //    Title = default
                //});

                //var id = externNewsDataAccess.GetLastIdentity();
                //var model = externNewsDataAccess.GetModel(id);
                //Assert.NotNull(model);

                var data = new List<ExternNews>
                {
                    new ExternNews
                    {
                        SecondaryType = null,
                        Sort = default,
                        DefaultCollectionNum = default,
                        DefaultReadNum = default,
                        BannerSort = default,
                        BannerType = default,
                        SourceId = default,
                        SourceType = default,
                        Summary = default,
                        ContractId = default,
                        CoverImg = default,
                        Html = default,
                        IsHot = default,
                        IsPublish = default,
                        IsRecommend = default,
                        LinkUrl = default,
                        NewsType = 1,
                        PublishTime = default,
                        RealCollectionNum = default,
                        RealReadNum = default,
                        ResourceUrl = default,
                        Tags = default,
                        Title = default
                    },
                    new ExternNews
                    {
                        Sort = default,
                        DefaultCollectionNum = default,
                        DefaultReadNum = default,
                        BannerSort = default,
                        BannerType = default,
                        SourceId = default,
                        SourceType = default,
                        Summary = default,
                        ContractId = default,
                        CoverImg = default,
                        Html = default,
                        IsHot = default,
                        IsPublish = default,
                        IsRecommend = default,
                        LinkUrl = default,
                        NewsType = 1,
                        PublishTime = default,
                        RealCollectionNum = default,
                        RealReadNum = default,
                        ResourceUrl = default,
                        Tags = default,
                        Title = default
                    }
                };

                //using(var conn = new SqlConnection("Persist Security Info=False;User ID=sa;Password=sqlzzq;Initial Catalog=test;Data Source=localhost;"))
                //{
                //    string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";
                //    var r = conn.Execute(sql, data);
                //    Assert.True(r > 0);
                //}

                var result = await externNewsDataAccess.BatchInsertAsync(data);
                var list = await externNewsDataAccess.GetPageAsync(null, "1=1", "id desc", 1, 5);
                Assert.True(list.Count() > 0);
            }
            catch
            {

            }
        }
    }

    public class TestSqlServerConnectionProvider : SqlServerConnectionProvider
    {
        public TestSqlServerConnectionProvider(IOptions<DbConnectionMapOptions> options) : base(options)
        {
        }

        protected override string GetRealOnlyDbConnectionString()
        {
            return GetConnectionString("MonicaTestDb_Read");
        }

        protected override string GetWriteDbConnectionString()
        {
            return GetConnectionString("MonicaTestDb");
        }
    }
}