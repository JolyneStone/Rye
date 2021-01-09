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
	public partial class DaoMenu : IMenu
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoMenu(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }

		public int GetLastIdentity()
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
			return conn.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
		}

        public int Insert(Menu model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(Menu model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(Menu model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn);
        }

        public async Task<int> InsertAsync(Menu model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn);
        }

        public int BatchInsert(IEnumerable<Menu> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<Menu> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<Menu> items)
        {
        	string sql = "INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<Menu> items)
        {
        	string sql = "INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime) VALUES (@Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(Menu model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE menu SET  name=@Name, path=@Path, sort=@Sort, icon=@Icon, status=@Status, parentId=@ParentId, appId=@AppId, updateTime=@UpdateTime WHERE 1=1  AND id=@Id;INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime); SELECT @Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime WHERE NOT EXISTS (SELECT 1 FROM menu where 1=1  AND id=@Id)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(Menu model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE menu SET  name=@Name, path=@Path, sort=@Sort, icon=@Icon, status=@Status, parentId=@ParentId, appId=@AppId, updateTime=@UpdateTime WHERE 1=1  AND id=@Id;INSERT INTO menu (name,path,sort,icon,status,parentId,appId,updateTime); SELECT @Name,@Path,@Sort,@Icon,@Status,@ParentId,@AppId,@UpdateTime WHERE NOT EXISTS (SELECT 1 FROM menu where 1=1  AND id=@Id)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(Menu model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            {
                return InsertUpdate(model, null, conn);
            }
        }
        
        public async Task<int> InsertUpdateAsync(Menu model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn);
        }
        
        public int Update(Menu model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE menu SET  name=@Name, path=@Path, sort=@Sort, icon=@Icon, status=@Status, parentId=@ParentId, appId=@AppId, updateTime=@UpdateTime WHERE 1=1  AND id=@Id";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(Menu model)
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn);
		}
        
        public async Task<int> UpdateAsync(Menu model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "GetUpdateSql()";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(Menu model)
		{
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn);
        }

        public bool Delete(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM menu WHERE 1=1 AND id=@Id";
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
            string sql = "DELETE FROM menu WHERE 1=1 AND id=@Id";
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

        public Menu GetModel(int id)
		{
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<Menu>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public Menu GetModelByWriteDb(int id)
		{
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<Menu>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<Menu> GetModelAsync(int id)
		{  
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<Menu> GetModelByWriteDbAsync(int id)
		{  
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
    		IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: _params, commandType: CommandType.Text);
		}	

        public Menu GetModel(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<Menu>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<Menu>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<Menu> GetModelAsync(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public Menu GetModel(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Menu> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public Menu GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Menu> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public Menu GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<Menu> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public Menu FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Menu> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public Menu FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<Menu> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public Menu FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<Menu>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<Menu> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<Menu>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<Menu> GetList()
        {
            string sql = "GetSelectAllSql()";
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<Menu>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Menu>> GetListAsync()
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  ORDER BY id DESC";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<Menu>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Menu> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "GetSelectAllSql()";
                
            if (trans == null)
                return conn.Query<Menu>(sql, commandType: CommandType.Text);

            else
                return conn.Query<Menu>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<Menu>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  ORDER BY id DESC";
                
            return await conn.QueryAsync<Menu>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Menu> GetListByWriteDb()
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  ORDER BY id DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<Menu>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Menu>> GetListByWriteDbAsync()
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  ORDER BY id DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<Menu>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Menu> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  ORDER BY id DESC";
                
            if (trans == null)
                return conn.Query<Menu>(sql, commandType: CommandType.Text);

            else
                return conn.Query<Menu>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<Menu>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  ORDER BY id DESC";
                
            return await conn.QueryAsync<Menu>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<Menu> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Menu>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<Menu> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Menu>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<Menu>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<Menu> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<Menu>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<Menu>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<Menu>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime FROM menu  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<Menu>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<Menu>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(int id)
        {
            string sql = "SELECT 1 FROM menu  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(int id)
        {
            string sql = "SELECT 1 FROM menu  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            string sql = "SELECT 1 FROM menu  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(int id)
        {
            string sql = "SELECT 1 FROM menu  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM menu  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(int id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM menu  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [menu] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [menu] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [menu] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [menu] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [menu] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [menu] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM menu ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM menu ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM menu ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM menu ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM menu  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM menu  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM menu  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM menu  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM menu ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM menu ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM menu WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM menu WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "id Id,name Name,path Path,sort Sort,icon Icon,status Status,parentId ParentId,appId AppId,updateTime UpdateTime";
        }
	}
}
