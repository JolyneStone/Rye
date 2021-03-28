using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Rye.DataAccess;
using Rye.MySql;

namespace Demo.DataAccess
{
	public partial class DaoAppInfo : IAppInfo
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoAppInfo(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }

		public int GetLastIdentity()
		{
			using Connector conn = ConnectionProvider.GetConnection();
			return conn.Connection.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
		}

        public int Insert(AppInfo model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(AppInfo model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(AppInfo model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn.Connection);
        }

        public async Task<int> InsertAsync(AppInfo model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn.Connection);
        }

        public int BatchInsert(IEnumerable<AppInfo> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<AppInfo> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<AppInfo> items)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<AppInfo> items)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(AppInfo model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE appInfo SET  name=@Name, remark=@Remark, appKey=@AppKey, appSecret=@AppSecret, createTime=@CreateTime, status=@Status WHERE 1=1  AND appId=@AppId;INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) SELECT @Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status WHERE NOT EXISTS (SELECT 1 FROM appInfo where 1=1  AND appId=@AppId)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(AppInfo model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE appInfo SET  name=@Name, remark=@Remark, appKey=@AppKey, appSecret=@AppSecret, createTime=@CreateTime, status=@Status WHERE 1=1  AND appId=@AppId;INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) SELECT @Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status WHERE NOT EXISTS (SELECT 1 FROM appInfo where 1=1  AND appId=@AppId)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(AppInfo model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return InsertUpdate(model, null, conn.Connection);
        }
        
        public async Task<int> InsertUpdateAsync(AppInfo model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn.Connection);
        }
        
        public int Update(AppInfo model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE appInfo SET  name=@Name, remark=@Remark, appKey=@AppKey, appSecret=@AppSecret, createTime=@CreateTime, status=@Status WHERE 1=1  AND appId=@AppId";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(AppInfo model)
		{
			using Connector conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn.Connection);
		}
        
        public async Task<int> UpdateAsync(AppInfo model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE appInfo SET  name=@Name, remark=@Remark, appKey=@AppKey, appSecret=@AppSecret, createTime=@CreateTime, status=@Status WHERE 1=1  AND appId=@AppId";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(AppInfo model)
		{
            using Connector conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn.Connection);
        }

        public bool Delete(int appId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(int appId)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Delete(appId, null, conn.Connection);
        }

        public async Task<bool> DeleteAsync(int appId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(int appId)
        {   
            using Connector conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(appId, null, conn.Connection);
        }

        public AppInfo GetModel(int appId)
		{
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public AppInfo GetModelByWriteDb(int appId)
		{
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<AppInfo> GetModelAsync(int appId)
		{  
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<AppInfo> GetModelByWriteDbAsync(int appId)
		{  
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
    		using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}	

        public AppInfo GetModel(int appId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<AppInfo> GetModelAsync(int appId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public AppInfo GetModel(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public AppInfo GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public AppInfo GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<AppInfo> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public AppInfo FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public AppInfo FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public AppInfo FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<AppInfo> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<AppInfo> GetList()
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<AppInfo>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetListAsync()
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<AppInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            if (trans == null)
                return conn.Query<AppInfo>(sql, commandType: CommandType.Text);

            else
                return conn.Query<AppInfo>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<AppInfo>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            return await conn.QueryAsync<AppInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetListByWriteDb()
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<AppInfo>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetListByWriteDbAsync()
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<AppInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            if (trans == null)
                return conn.Query<AppInfo>(sql, commandType: CommandType.Text);

            else
                return conn.Query<AppInfo>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<AppInfo>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            return await conn.QueryAsync<AppInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<AppInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<AppInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<AppInfo>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<AppInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(int appId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(int appId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM appInfo WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM appInfo WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status";
        }
	}
}
