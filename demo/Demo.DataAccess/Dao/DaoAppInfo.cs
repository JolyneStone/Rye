using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Monica.DataAccess;
using Monica.MySql;

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
			IDbConnection conn = ConnectionProvider.GetConnection();
			return conn.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
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
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn);
        }

        public async Task<int> InsertAsync(AppInfo model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn);
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

            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<AppInfo> items)
        {
        	string sql = "INSERT INTO appInfo (name,remark,appKey,appSecret,createTime,status) VALUES (@Name,@Remark,@AppKey,@AppSecret,@CreateTime,@Status);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
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
            IDbConnection conn = ConnectionProvider.GetConnection();
            {
                return InsertUpdate(model, null, conn);
            }
        }
        
        public async Task<int> InsertUpdateAsync(AppInfo model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn);
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
			IDbConnection conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn);
		}
        
        public async Task<int> UpdateAsync(AppInfo model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "GetUpdateSql()";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(AppInfo model)
		{
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn);
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
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Delete(appId, null,conn);
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
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(appId, null,conn);
        }

        public AppInfo GetModel(int appId)
		{
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public AppInfo GetModelByWriteDb(int appId)
		{
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<AppInfo> GetModelAsync(int appId)
		{  
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<AppInfo> GetModelByWriteDbAsync(int appId)
		{  
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo WHERE 1=1 AND appId=@AppId";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
    		IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: _params, commandType: CommandType.Text);
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
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public AppInfo GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
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
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public AppInfo FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<AppInfo> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
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
            string sql = "GetSelectAllSql()";
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<AppInfo>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetListAsync()
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<AppInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "GetSelectAllSql()";
                
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
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<AppInfo>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetListByWriteDbAsync()
        {
            string sql = "SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  ORDER BY appId DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<AppInfo>(sql, commandType: CommandType.Text);
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
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<AppInfo> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<AppInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<AppInfo>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT appId AppId,name Name,remark Remark,appKey AppKey,appSecret AppSecret,createTime CreateTime,status Status FROM appInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<AppInfo>(sql, param: param, commandType: CommandType.Text);
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
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(int appId)
        {
            string sql = "SELECT 1 FROM appInfo  WHERE 1=1  AND appId=@AppId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@AppId", value: appId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
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
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [appInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
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
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM appInfo ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM appInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
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
