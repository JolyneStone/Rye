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
	public partial class DaoRolePermission : IRolePermission
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoRolePermission(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }


        public int Insert(RolePermission model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO rolePermission (roleId,permissionId) VALUES (@RoleId,@PermissionId);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(RolePermission model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO rolePermission (roleId,permissionId) VALUES (@RoleId,@PermissionId);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(RolePermission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn);
        }

        public async Task<int> InsertAsync(RolePermission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn);
        }

        public int BatchInsert(IEnumerable<RolePermission> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO rolePermission (roleId,permissionId) VALUES (@RoleId,@PermissionId);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<RolePermission> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO rolePermission (roleId,permissionId) VALUES (@RoleId,@PermissionId);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<RolePermission> items)
        {
        	string sql = "INSERT INTO rolePermission (roleId,permissionId) VALUES (@RoleId,@PermissionId);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<RolePermission> items)
        {
        	string sql = "INSERT INTO rolePermission (roleId,permissionId) VALUES (@RoleId,@PermissionId);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(RolePermission model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE rolePermission SET WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId;INSERT INTO rolePermission (roleId,permissionId) SELECT @RoleId,@PermissionId WHERE NOT EXISTS (SELECT 1 FROM rolePermission where 1=1  AND roleId=@RoleId AND permissionId=@PermissionId)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(RolePermission model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE rolePermission SET WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId;INSERT INTO rolePermission (roleId,permissionId) SELECT @RoleId,@PermissionId WHERE NOT EXISTS (SELECT 1 FROM rolePermission where 1=1  AND roleId=@RoleId AND permissionId=@PermissionId)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(RolePermission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            {
                return InsertUpdate(model, null, conn);
            }
        }
        
        public async Task<int> InsertUpdateAsync(RolePermission model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn);
        }
        
        public int Update(RolePermission model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(RolePermission model)
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn);
		}
        
        public async Task<int> UpdateAsync(RolePermission model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "GetUpdateSql()";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(RolePermission model)
		{
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn);
        }

        public bool Delete(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(int roleId,int permissionId)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Delete(roleId,permissionId, null,conn);
        }

        public async Task<bool> DeleteAsync(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(int roleId,int permissionId)
        {   
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(roleId,permissionId, null,conn);
        }

        public RolePermission GetModel(int roleId,int permissionId)
		{
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<RolePermission>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public RolePermission GetModelByWriteDb(int roleId,int permissionId)
		{
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<RolePermission>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<RolePermission> GetModelAsync(int roleId,int permissionId)
		{  
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<RolePermission> GetModelByWriteDbAsync(int roleId,int permissionId)
		{  
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
    		IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: _params, commandType: CommandType.Text);
		}	

        public RolePermission GetModel(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<RolePermission>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<RolePermission>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<RolePermission> GetModelAsync(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission WHERE 1=1 AND roleId=@RoleId AND permissionId=@PermissionId";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public RolePermission GetModel(object param, string whereSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<RolePermission> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public RolePermission GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<RolePermission> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public RolePermission GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<RolePermission> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public RolePermission FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<RolePermission> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public RolePermission FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<RolePermission> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public RolePermission FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<RolePermission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<RolePermission> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<RolePermission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<RolePermission> GetList()
        {
            string sql = "GetSelectAllSql()";
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<RolePermission>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<RolePermission>> GetListAsync()
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  ORDER BY roleId DESC,permissionId DESC";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<RolePermission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<RolePermission> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "GetSelectAllSql()";
                
            if (trans == null)
                return conn.Query<RolePermission>(sql, commandType: CommandType.Text);

            else
                return conn.Query<RolePermission>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<RolePermission>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  ORDER BY roleId DESC,permissionId DESC";
                
            return await conn.QueryAsync<RolePermission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<RolePermission> GetListByWriteDb()
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  ORDER BY roleId DESC,permissionId DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<RolePermission>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<RolePermission>> GetListByWriteDbAsync()
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  ORDER BY roleId DESC,permissionId DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<RolePermission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<RolePermission> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  ORDER BY roleId DESC,permissionId DESC";
                
            if (trans == null)
                return conn.Query<RolePermission>(sql, commandType: CommandType.Text);

            else
                return conn.Query<RolePermission>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<RolePermission>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  ORDER BY roleId DESC,permissionId DESC";
                
            return await conn.QueryAsync<RolePermission>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<RolePermission> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<RolePermission>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<RolePermission> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<RolePermission>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<RolePermission> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<RolePermission>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<RolePermission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<RolePermission>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT roleId RoleId,permissionId PermissionId FROM rolePermission  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<RolePermission>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<RolePermission>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(int roleId,int permissionId)
        {
            string sql = "SELECT 1 FROM rolePermission  WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(int roleId,int permissionId)
        {
            string sql = "SELECT 1 FROM rolePermission  WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(int roleId,int permissionId)
        {
            string sql = "SELECT 1 FROM rolePermission  WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(int roleId,int permissionId)
        {
            string sql = "SELECT 1 FROM rolePermission  WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM rolePermission  WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM rolePermission  WHERE 1=1  AND roleId=@RoleId AND permissionId=@PermissionId LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@RoleId", value: roleId, direction: ParameterDirection.Input);
			_params.Add("@PermissionId", value: permissionId, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [rolePermission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [rolePermission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [rolePermission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [rolePermission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [rolePermission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [rolePermission] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM rolePermission ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM rolePermission ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM rolePermission ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM rolePermission ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM rolePermission WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "roleId RoleId,permissionId PermissionId";
        }
	}
}
