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
	public partial class DaoPermission : IPermission
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoPermission(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }

		public int GetLastIdentity()
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
			return conn.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
		}

        public int Insert(Permission model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(Permission model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(Permission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn);
        }

        public async Task<int> InsertAsync(Permission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn);
        }

        public int BatchInsert(IEnumerable<Permission> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<Permission> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<Permission> items)
        {
        	string sql = "INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<Permission> items)
        {
        	string sql = "INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(Permission model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE permission SET  name=@Name, code=@Code, sort=@Sort, icon=@Icon, status=@Status, parentId=@ParentId, appId=@AppId, updateTime=@UpdateTime WHERE 1=1  AND id=@Id;INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) SELECT @Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime WHERE NOT EXISTS (SELECT 1 FROM permission where 1=1  AND id=@Id)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(Permission model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE permission SET  name=@Name, code=@Code, sort=@Sort, icon=@Icon, status=@Status, parentId=@ParentId, appId=@AppId, updateTime=@UpdateTime WHERE 1=1  AND id=@Id;INSERT INTO permission (name,code,sort,icon,status,parentId,appId,updateTime) SELECT @Name,@Code,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime WHERE NOT EXISTS (SELECT 1 FROM permission where 1=1  AND id=@Id)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(Permission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            {
                return InsertUpdate(model, null, conn);
            }
        }
        
        public async Task<int> InsertUpdateAsync(Permission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn);
        }
        
        public int Update(Permission model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE permission SET  name=@Name, code=@Code, sort=@Sort, icon=@Icon, status=@Status, parentId=@ParentId, appId=@AppId, updateTime=@UpdateTime WHERE 1=1  AND id=@Id";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(Permission model)
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn);
		}
        
        public async Task<int> UpdateAsync(Permission model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "GetUpdateSql()";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(Permission model)
		{
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn);
        }

        public bool Delete(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(int id)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Delete(id, null,conn);
        }

        public async Task<bool> DeleteAsync(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {   
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(id, null,conn);
        }

        public Permission GetModel(int id)
		{
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<Permission>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public Permission GetModelByWriteDb(int id)
		{
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<Permission>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<Permission> GetModelAsync(int id)
		{  
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<Permission> GetModelByWriteDbAsync(int id)
		{  
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
    		IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: _params, commandType: CommandType.Text);
		}	

        public Permission GetModel(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<Permission>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<Permission>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<Permission> GetModelAsync(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public Permission GetModel(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Permission> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public Permission GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Permission> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public Permission GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<Permission> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public Permission FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Permission> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public Permission FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Permission> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public Permission FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<Permission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<Permission> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<Permission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<Permission> GetList()
        {
            string sql = "GetSelectAllSql()";
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<Permission>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Permission>> GetListAsync()
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  ORDER BY id DESC";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<Permission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Permission> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "GetSelectAllSql()";
                
            if (trans == null)
                return conn.Query<Permission>(sql, commandType: CommandType.Text);

            else
                return conn.Query<Permission>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<Permission>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  ORDER BY id DESC";
                
            return await conn.QueryAsync<Permission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Permission> GetListByWriteDb()
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  ORDER BY id DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<Permission>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Permission>> GetListByWriteDbAsync()
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  ORDER BY id DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<Permission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Permission> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  ORDER BY id DESC";
                
            if (trans == null)
                return conn.Query<Permission>(sql, commandType: CommandType.Text);

            else
                return conn.Query<Permission>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<Permission>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  ORDER BY id DESC";
                
            return await conn.QueryAsync<Permission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Permission> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Permission>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<Permission> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Permission>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<Permission>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<Permission> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<Permission>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<Permission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<Permission>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM permission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<Permission>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<Permission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(int id)
        {
            string sql = "SELECT 1 FROM permission  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(int id)
        {
            string sql = "SELECT 1 FROM permission  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            string sql = "SELECT 1 FROM permission  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(int id)
        {
            string sql = "SELECT 1 FROM permission  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM permission  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM permission  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [permission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [permission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [permission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [permission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [permission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [permission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM permission ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM permission ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM permission ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM permission ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM permission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM permission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM permission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM permission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM permission ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM permission ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM permission WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM permission WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "id Id,name Name,code Code,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime";
        }
	}
}
