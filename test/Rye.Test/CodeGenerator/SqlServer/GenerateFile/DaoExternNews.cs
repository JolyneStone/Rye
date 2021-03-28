using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Rye.DataAccess;
using Rye.SqlServer;

namespace Rye.DataAccess.Model
{
	public partial class DaoExternNews : IExternNews
    {
        public SqlServerConnectionProvider ConnectionProvider { get; }

        public DaoExternNews(SqlServerConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }

		public long GetLastIdentity()
		{
			using Connector conn = ConnectionProvider.GetConnection();
			return conn.Connection.ExecuteScalar<long>("SELECT SCOPE_IDENTITY()");
		}

        public int Insert(ExternNews model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(ExternNews model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(ExternNews model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn.Connection);
        }

        public async Task<int> InsertAsync(ExternNews model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn.Connection);
        }

        public int BatchInsert(IEnumerable<ExternNews> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<ExternNews> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<ExternNews> items)
        {
        	string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";

            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<ExternNews> items)
        {
        	string sql = "INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) VALUES (@SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId);";

            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(ExternNews model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE [ExternNews] SET  [SourceType]=@SourceType, [SourceId]=@SourceId, [IsPublish]=@IsPublish, [PublishTime]=@PublishTime, [BannerType]=@BannerType, [IsHot]=@IsHot, [IsRecommend]=@IsRecommend, [Sort]=@Sort, [RealReadNum]=@RealReadNum, [DefaultReadNum]=@DefaultReadNum, [RealCollectionNum]=@RealCollectionNum, [DefaultCollectionNum]=@DefaultCollectionNum, [Title]=@Title, [Summary]=@Summary, [LinkUrl]=@LinkUrl, [Tags]=@Tags, [CoverImg]=@CoverImg, [Html]=@Html, [BannerSort]=@BannerSort, [NewsType]=@NewsType, [secondaryType]=@SecondaryType, [resourceUrl]=@ResourceUrl, [ContractId]=@ContractId WHERE 1=1  AND [Id]=@Id;INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) SELECT @SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId WHERE NOT EXISTS (SELECT 1 FROM [ExternNews] where 1=1  AND [Id]=@Id)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(ExternNews model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE [ExternNews] SET  [SourceType]=@SourceType, [SourceId]=@SourceId, [IsPublish]=@IsPublish, [PublishTime]=@PublishTime, [BannerType]=@BannerType, [IsHot]=@IsHot, [IsRecommend]=@IsRecommend, [Sort]=@Sort, [RealReadNum]=@RealReadNum, [DefaultReadNum]=@DefaultReadNum, [RealCollectionNum]=@RealCollectionNum, [DefaultCollectionNum]=@DefaultCollectionNum, [Title]=@Title, [Summary]=@Summary, [LinkUrl]=@LinkUrl, [Tags]=@Tags, [CoverImg]=@CoverImg, [Html]=@Html, [BannerSort]=@BannerSort, [NewsType]=@NewsType, [secondaryType]=@SecondaryType, [resourceUrl]=@ResourceUrl, [ContractId]=@ContractId WHERE 1=1  AND [Id]=@Id;INSERT INTO [ExternNews] ([SourceType],[SourceId],[IsPublish],[PublishTime],[BannerType],[IsHot],[IsRecommend],[Sort],[RealReadNum],[DefaultReadNum],[RealCollectionNum],[DefaultCollectionNum],[Title],[Summary],[LinkUrl],[Tags],[CoverImg],[Html],[BannerSort],[NewsType],[secondaryType],[resourceUrl],[ContractId]) SELECT @SourceType,@SourceId,@IsPublish,@PublishTime,@BannerType,@IsHot,@IsRecommend,@Sort,@RealReadNum,@DefaultReadNum,@RealCollectionNum,@DefaultCollectionNum,@Title,@Summary,@LinkUrl,@Tags,@CoverImg,@Html,@BannerSort,@NewsType,@SecondaryType,@ResourceUrl,@ContractId WHERE NOT EXISTS (SELECT 1 FROM [ExternNews] where 1=1  AND [Id]=@Id)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(ExternNews model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return InsertUpdate(model, null, conn.Connection);
        }
        
        public async Task<int> InsertUpdateAsync(ExternNews model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn.Connection);
        }
        
        public int Update(ExternNews model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE [ExternNews] SET  [SourceType]=@SourceType, [SourceId]=@SourceId, [IsPublish]=@IsPublish, [PublishTime]=@PublishTime, [BannerType]=@BannerType, [IsHot]=@IsHot, [IsRecommend]=@IsRecommend, [Sort]=@Sort, [RealReadNum]=@RealReadNum, [DefaultReadNum]=@DefaultReadNum, [RealCollectionNum]=@RealCollectionNum, [DefaultCollectionNum]=@DefaultCollectionNum, [Title]=@Title, [Summary]=@Summary, [LinkUrl]=@LinkUrl, [Tags]=@Tags, [CoverImg]=@CoverImg, [Html]=@Html, [BannerSort]=@BannerSort, [NewsType]=@NewsType, [secondaryType]=@SecondaryType, [resourceUrl]=@ResourceUrl, [ContractId]=@ContractId WHERE 1=1  AND [Id]=@Id";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(ExternNews model)
		{
			using Connector conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn.Connection);
		}
        
        public async Task<int> UpdateAsync(ExternNews model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE [ExternNews] SET  [SourceType]=@SourceType, [SourceId]=@SourceId, [IsPublish]=@IsPublish, [PublishTime]=@PublishTime, [BannerType]=@BannerType, [IsHot]=@IsHot, [IsRecommend]=@IsRecommend, [Sort]=@Sort, [RealReadNum]=@RealReadNum, [DefaultReadNum]=@DefaultReadNum, [RealCollectionNum]=@RealCollectionNum, [DefaultCollectionNum]=@DefaultCollectionNum, [Title]=@Title, [Summary]=@Summary, [LinkUrl]=@LinkUrl, [Tags]=@Tags, [CoverImg]=@CoverImg, [Html]=@Html, [BannerSort]=@BannerSort, [NewsType]=@NewsType, [secondaryType]=@SecondaryType, [resourceUrl]=@ResourceUrl, [ContractId]=@ContractId WHERE 1=1  AND [Id]=@Id";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(ExternNews model)
		{
            using Connector conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn.Connection);
        }

        public bool Delete(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM [ExternNews] WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(long id)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Delete(id, null, conn.Connection);
        }

        public async Task<bool> DeleteAsync(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM [ExternNews] WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {   
            using Connector conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(id, null, conn.Connection);
        }

        public ExternNews GetModel(long id)
		{
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<ExternNews>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public ExternNews GetModelByWriteDb(long id)
		{
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<ExternNews>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<ExternNews> GetModelAsync(long id)
		{  
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ExternNews>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<ExternNews> GetModelByWriteDbAsync(long id)
		{  
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
    		using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ExternNews>(sql, param: _params, commandType: CommandType.Text);
		}	

        public ExternNews GetModel(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<ExternNews>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<ExternNews>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<ExternNews> GetModelAsync(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<ExternNews>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<ExternNews>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public ExternNews GetModel(object param, string whereSql)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ExternNews> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public ExternNews GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ExternNews> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public ExternNews GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            
            if (trans == null)
                return conn.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<ExternNews> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public ExternNews FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT TOP 1 [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql;
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ExternNews> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT TOP 1 [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql;
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public ExternNews FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT TOP 1 [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql;
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ExternNews> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT TOP 1 [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql;
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public ExternNews FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT TOP 1 [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql;
            
            if (trans == null)
                return conn.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<ExternNews>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<ExternNews> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT TOP 1 [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql;
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<ExternNews>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<ExternNews> GetList()
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<ExternNews>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ExternNews>> GetListAsync()
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<ExternNews>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ExternNews> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            if (trans == null)
                return conn.Query<ExternNews>(sql, commandType: CommandType.Text);

            else
                return conn.Query<ExternNews>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<ExternNews>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            return await conn.QueryAsync<ExternNews>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ExternNews> GetListByWriteDb()
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<ExternNews>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ExternNews>> GetListByWriteDbAsync()
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<ExternNews>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ExternNews> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            if (trans == null)
                return conn.Query<ExternNews>(sql, commandType: CommandType.Text);

            else
                return conn.Query<ExternNews>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<ExternNews>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) ORDER BY [Id] DESC";
                
            return await conn.QueryAsync<ExternNews>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ExternNews> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ExternNews>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<ExternNews> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ExternNews>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<ExternNews> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<ExternNews>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<ExternNews>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<ExternNews>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT [Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId FROM [ExternNews] WITH(NOLOCK) WHERE {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<ExternNews>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<ExternNews>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(long id)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1  AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(long id)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1  AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1  AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(long id)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1  AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1  AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1  AND [Id]=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT TOP 1 1 FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) ";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) ";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK)  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK)  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK)  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK)  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM [ExternNews] WITH(NOLOCK) WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "[Id] Id,[SourceType] SourceType,[SourceId] SourceId,[IsPublish] IsPublish,[PublishTime] PublishTime,[BannerType] BannerType,[IsHot] IsHot,[IsRecommend] IsRecommend,[Sort] Sort,[RealReadNum] RealReadNum,[DefaultReadNum] DefaultReadNum,[RealCollectionNum] RealCollectionNum,[DefaultCollectionNum] DefaultCollectionNum,[Title] Title,[Summary] Summary,[LinkUrl] LinkUrl,[Tags] Tags,[CoverImg] CoverImg,[Html] Html,[BannerSort] BannerSort,[NewsType] NewsType,[secondaryType] SecondaryType,[resourceUrl] ResourceUrl,[ContractId] ContractId";
        }
	}
}
