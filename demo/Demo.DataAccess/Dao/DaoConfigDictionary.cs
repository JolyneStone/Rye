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
	public partial class DaoConfigDictionary : IConfigDictionary
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoConfigDictionary(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }


        public int Insert(ConfigDictionary model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO configDictionary (dicKey,dicValue) VALUES (@DicKey,@DicValue);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(ConfigDictionary model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO configDictionary (dicKey,dicValue) VALUES (@DicKey,@DicValue);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(ConfigDictionary model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn.Connection);
        }

        public async Task<int> InsertAsync(ConfigDictionary model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn.Connection);
        }

        public int BatchInsert(IEnumerable<ConfigDictionary> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO configDictionary (dicKey,dicValue) VALUES (@DicKey,@DicValue);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<ConfigDictionary> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO configDictionary (dicKey,dicValue) VALUES (@DicKey,@DicValue);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<ConfigDictionary> items)
        {
        	string sql = "INSERT INTO configDictionary (dicKey,dicValue) VALUES (@DicKey,@DicValue);";

            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<ConfigDictionary> items)
        {
        	string sql = "INSERT INTO configDictionary (dicKey,dicValue) VALUES (@DicKey,@DicValue);";

            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(ConfigDictionary model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE configDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey;INSERT INTO configDictionary (dicKey,dicValue) SELECT @DicKey,@DicValue WHERE NOT EXISTS (SELECT 1 FROM configDictionary where 1=1  AND dicKey=@DicKey)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(ConfigDictionary model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE configDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey;INSERT INTO configDictionary (dicKey,dicValue) SELECT @DicKey,@DicValue WHERE NOT EXISTS (SELECT 1 FROM configDictionary where 1=1  AND dicKey=@DicKey)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(ConfigDictionary model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return InsertUpdate(model, null, conn.Connection);
        }
        
        public async Task<int> InsertUpdateAsync(ConfigDictionary model)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn.Connection);
        }
        
        public int Update(ConfigDictionary model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE configDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(ConfigDictionary model)
		{
			using Connector conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn.Connection);
		}
        
        public async Task<int> UpdateAsync(ConfigDictionary model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE configDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(ConfigDictionary model)
		{
            using Connector conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn.Connection);
        }

        public bool Delete(string dicKey, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(string dicKey)
        {
            using Connector conn = ConnectionProvider.GetConnection();
            return Delete(dicKey, null, conn.Connection);
        }

        public async Task<bool> DeleteAsync(string dicKey, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(string dicKey)
        {   
            using Connector conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(dicKey, null, conn.Connection);
        }

        public ConfigDictionary GetModel(string dicKey)
		{
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public ConfigDictionary GetModelByWriteDb(string dicKey)
		{
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<ConfigDictionary> GetModelAsync(string dicKey)
		{  
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<ConfigDictionary> GetModelByWriteDbAsync(string dicKey)
		{  
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
    		using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text);
		}	

        public ConfigDictionary GetModel(string dicKey, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<ConfigDictionary> GetModelAsync(string dicKey, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary WHERE 1=1 AND dicKey=@DicKey";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public ConfigDictionary GetModel(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ConfigDictionary> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public ConfigDictionary GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ConfigDictionary> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public ConfigDictionary GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<ConfigDictionary> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public ConfigDictionary FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ConfigDictionary> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public ConfigDictionary FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<ConfigDictionary> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public ConfigDictionary FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<ConfigDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<ConfigDictionary> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<ConfigDictionary> GetList()
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<ConfigDictionary>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ConfigDictionary>> GetListAsync()
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
            
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<ConfigDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ConfigDictionary> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            if (trans == null)
                return conn.Query<ConfigDictionary>(sql, commandType: CommandType.Text);

            else
                return conn.Query<ConfigDictionary>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<ConfigDictionary>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            return await conn.QueryAsync<ConfigDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ConfigDictionary> GetListByWriteDb()
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<ConfigDictionary>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ConfigDictionary>> GetListByWriteDbAsync()
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<ConfigDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ConfigDictionary> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            if (trans == null)
                return conn.Query<ConfigDictionary>(sql, commandType: CommandType.Text);

            else
                return conn.Query<ConfigDictionary>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<ConfigDictionary>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  ORDER BY dicKey DESC";
                
            return await conn.QueryAsync<ConfigDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<ConfigDictionary> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.Query<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ConfigDictionary>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.QueryAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<ConfigDictionary> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.Query<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<ConfigDictionary>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.QueryAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<ConfigDictionary> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<ConfigDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<ConfigDictionary>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT dicKey DicKey,dicValue DicValue FROM configDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<ConfigDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(string dicKey)
        {
            string sql = "SELECT 1 FROM configDictionary  WHERE 1=1  AND dicKey=@DicKey LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(string dicKey)
        {
            string sql = "SELECT 1 FROM configDictionary  WHERE 1=1  AND dicKey=@DicKey LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(string dicKey)
        {
            string sql = "SELECT 1 FROM configDictionary  WHERE 1=1  AND dicKey=@DicKey LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(string dicKey)
        {
            string sql = "SELECT 1 FROM configDictionary  WHERE 1=1  AND dicKey=@DicKey LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(string dicKey, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM configDictionary  WHERE 1=1  AND dicKey=@DicKey LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(string dicKey, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM configDictionary  WHERE 1=1  AND dicKey=@DicKey LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [configDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [configDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [configDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [configDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [configDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [configDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM configDictionary ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM configDictionary ";
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM configDictionary ";
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM configDictionary ";
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return conn.Connection.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary  WHERE 1=1 AND " + whereSql;
            using Connector conn = ConnectionProvider.GetConnection();
            return await conn.Connection.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM configDictionary WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "dicKey DicKey,dicValue DicValue";
        }
	}
}
