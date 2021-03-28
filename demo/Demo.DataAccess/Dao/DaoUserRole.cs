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
	public partial class DaoUserRole : IUserRole
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoUserRole(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }


        public int Insert(UserRole model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userRole (userId,roleId) VALUES (@UserId,@RoleId);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(UserRole model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userRole (userId,roleId) VALUES (@UserId,@RoleId);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(UserRole model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn.Connection);
        }

        public async Task<int> InsertAsync(UserRole model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn.Connection);
        }

        public int BatchInsert(IEnumerable<UserRole> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userRole (userId,roleId) VALUES (@UserId,@RoleId);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<UserRole> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userRole (userId,roleId) VALUES (@UserId,@RoleId);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<UserRole> items)
        {
        	string sql = "INSERT INTO userRole (userId,roleId) VALUES (@UserId,@RoleId);";

            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<UserRole> items)
        {
        	string sql = "INSERT INTO userRole (userId,roleId) VALUES (@UserId,@RoleId);";

            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(UserRole model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE userRole SET WHERE 1=1  AND userId=@UserId AND roleId=@RoleId;INSERT INTO userRole (userId,roleId) SELECT @UserId,@RoleId WHERE NOT EXISTS (SELECT 1 FROM userRole where 1=1  AND userId=@UserId AND roleId=@RoleId)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(UserRole model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE userRole SET WHERE 1=1  AND userId=@UserId AND roleId=@RoleId;INSERT INTO userRole (userId,roleId) SELECT @UserId,@RoleId WHERE NOT EXISTS (SELECT 1 FROM userRole where 1=1  AND userId=@UserId AND roleId=@RoleId)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(UserRole model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return InsertUpdate(model, null, conn.Connection);
        }
        
        public async Task<int> InsertUpdateAsync(UserRole model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn.Connection);
        }
        
        public int Update(UserRole model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(UserRole model)
		{
			using Connector conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn.Connection);
		}
        
        public async Task<int> UpdateAsync(UserRole model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(UserRole model)
		{
            using Connector conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn.Connection);
        }

        public bool Delete(int userId,int roleId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(int userId,int roleId)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Delete(userId,roleId, null, conn.Connection);
        }

        public async Task<bool> DeleteAsync(int userId,int roleId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(int userId,int roleId)
        {   
            using Connector conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(userId,roleId, null, conn.Connection);
        }

        public UserRole GetModel(int userId,int roleId)
		{
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<UserRole>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public UserRole GetModelByWriteDb(int userId,int roleId)
		{
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<UserRole>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<UserRole> GetModelAsync(int userId,int roleId)
		{  
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<UserRole>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<UserRole> GetModelByWriteDbAsync(int userId,int roleId)
		{  
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
    		using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<UserRole>(sql, param: _params, commandType: CommandType.Text);
		}	

        public UserRole GetModel(int userId,int roleId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<UserRole>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<UserRole>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<UserRole> GetModelAsync(int userId,int roleId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole WHERE 1=1 AND userId=@UserId AND roleId=@RoleId";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<UserRole>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<UserRole>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public UserRole GetModel(object param, string whereSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserRole> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public UserRole GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserRole> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public UserRole GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<UserRole> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public UserRole FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserRole> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public UserRole FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserRole> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public UserRole FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<UserRole>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<UserRole> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<UserRole>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<UserRole> GetList()
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<UserRole>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserRole>> GetListAsync()
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<UserRole>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserRole> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            if (trans == null)
                return conn.Query<UserRole>(sql, commandType: CommandType.Text);

            else
                return conn.Query<UserRole>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<UserRole>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            return await conn.QueryAsync<UserRole>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserRole> GetListByWriteDb()
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<UserRole>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserRole>> GetListByWriteDbAsync()
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<UserRole>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserRole> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            if (trans == null)
                return conn.Query<UserRole>(sql, commandType: CommandType.Text);

            else
                return conn.Query<UserRole>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<UserRole>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT userId UserId,roleId RoleId FROM userRole  ORDER BY userId DESC,roleId DESC";
                
            return await conn.QueryAsync<UserRole>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserRole> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT userId UserId,roleId RoleId FROM userRole  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserRole>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT userId UserId,roleId RoleId FROM userRole  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<UserRole> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT userId UserId,roleId RoleId FROM userRole  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserRole>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT userId UserId,roleId RoleId FROM userRole  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<UserRole>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<UserRole> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT userId UserId,roleId RoleId FROM userRole  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<UserRole>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<UserRole>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<UserRole>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT userId UserId,roleId RoleId FROM userRole  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<UserRole>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<UserRole>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(int userId,int roleId)
        {
            string sql = "SELECT 1 FROM userRole  WHERE 1=1  AND userId=@UserId AND roleId=@RoleId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(int userId,int roleId)
        {
            string sql = "SELECT 1 FROM userRole  WHERE 1=1  AND userId=@UserId AND roleId=@RoleId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(int userId,int roleId)
        {
            string sql = "SELECT 1 FROM userRole  WHERE 1=1  AND userId=@UserId AND roleId=@RoleId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(int userId,int roleId)
        {
            string sql = "SELECT 1 FROM userRole  WHERE 1=1  AND userId=@UserId AND roleId=@RoleId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(int userId,int roleId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM userRole  WHERE 1=1  AND userId=@UserId AND roleId=@RoleId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(int userId,int roleId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM userRole  WHERE 1=1  AND userId=@UserId AND roleId=@RoleId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@UserId", value: userId, direction: ParameterDirection.Input);
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userRole] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userRole] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userRole] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userRole] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [userRole] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [userRole] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM userRole ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM userRole ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM userRole ";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM userRole ";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userRole  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userRole  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userRole  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userRole  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userRole ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userRole ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userRole WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userRole WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "userId UserId,roleId RoleId";
        }
	}
}
