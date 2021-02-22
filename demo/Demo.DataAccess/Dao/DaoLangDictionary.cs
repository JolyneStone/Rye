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
	public partial class DaoLangDictionary : ILangDictionary
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoLangDictionary(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }


        public int Insert(LangDictionary model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO langDictionary (dicKey,lang,dicValue) VALUES (@DicKey,@Lang,@DicValue);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(LangDictionary model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO langDictionary (dicKey,lang,dicValue) VALUES (@DicKey,@Lang,@DicValue);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(LangDictionary model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn);
        }

        public async Task<int> InsertAsync(LangDictionary model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn);
        }

        public int BatchInsert(IEnumerable<LangDictionary> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO langDictionary (dicKey,lang,dicValue) VALUES (@DicKey,@Lang,@DicValue);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<LangDictionary> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO langDictionary (dicKey,lang,dicValue) VALUES (@DicKey,@Lang,@DicValue);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<LangDictionary> items)
        {
        	string sql = "INSERT INTO langDictionary (dicKey,lang,dicValue) VALUES (@DicKey,@Lang,@DicValue);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<LangDictionary> items)
        {
        	string sql = "INSERT INTO langDictionary (dicKey,lang,dicValue) VALUES (@DicKey,@Lang,@DicValue);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(LangDictionary model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE langDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang;INSERT INTO langDictionary (dicKey,lang,dicValue) SELECT @DicKey,@Lang,@DicValue WHERE NOT EXISTS (SELECT 1 FROM langDictionary where 1=1  AND dicKey=@DicKey AND lang=@Lang)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(LangDictionary model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE langDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang;INSERT INTO langDictionary (dicKey,lang,dicValue) SELECT @DicKey,@Lang,@DicValue WHERE NOT EXISTS (SELECT 1 FROM langDictionary where 1=1  AND dicKey=@DicKey AND lang=@Lang)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(LangDictionary model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            {
                return InsertUpdate(model, null, conn);
            }
        }
        
        public async Task<int> InsertUpdateAsync(LangDictionary model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn);
        }
        
        public int Update(LangDictionary model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE langDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(LangDictionary model)
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn);
		}
        
        public async Task<int> UpdateAsync(LangDictionary model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE langDictionary SET  dicValue=@DicValue WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(LangDictionary model)
		{
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn);
        }

        public bool Delete(string dicKey,string lang, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(string dicKey,string lang)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Delete(dicKey,lang, null,conn);
        }

        public async Task<bool> DeleteAsync(string dicKey,string lang, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(string dicKey,string lang)
        {   
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(dicKey,lang, null,conn);
        }

        public LangDictionary GetModel(string dicKey,string lang)
		{
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<LangDictionary>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public LangDictionary GetModelByWriteDb(string dicKey,string lang)
		{
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<LangDictionary>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<LangDictionary> GetModelAsync(string dicKey,string lang)
		{  
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<LangDictionary> GetModelByWriteDbAsync(string dicKey,string lang)
		{  
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
    		IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: _params, commandType: CommandType.Text);
		}	

        public LangDictionary GetModel(string dicKey,string lang, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<LangDictionary>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<LangDictionary>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<LangDictionary> GetModelAsync(string dicKey,string lang, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary WHERE 1=1 AND dicKey=@DicKey AND lang=@Lang";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public LangDictionary GetModel(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<LangDictionary> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public LangDictionary GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<LangDictionary> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public LangDictionary GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<LangDictionary> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public LangDictionary FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<LangDictionary> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public LangDictionary FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<LangDictionary> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public LangDictionary FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<LangDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<LangDictionary> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<LangDictionary> GetList()
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<LangDictionary>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<LangDictionary>> GetListAsync()
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<LangDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<LangDictionary> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            if (trans == null)
                return conn.Query<LangDictionary>(sql, commandType: CommandType.Text);

            else
                return conn.Query<LangDictionary>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<LangDictionary>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            return await conn.QueryAsync<LangDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<LangDictionary> GetListByWriteDb()
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<LangDictionary>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<LangDictionary>> GetListByWriteDbAsync()
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<LangDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<LangDictionary> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            if (trans == null)
                return conn.Query<LangDictionary>(sql, commandType: CommandType.Text);

            else
                return conn.Query<LangDictionary>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<LangDictionary>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  ORDER BY dicKey DESC,lang DESC";
                
            return await conn.QueryAsync<LangDictionary>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<LangDictionary> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<LangDictionary>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<LangDictionary> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<LangDictionary>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<LangDictionary> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<LangDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<LangDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<LangDictionary>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT dicKey DicKey,lang Lang,dicValue DicValue FROM langDictionary  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<LangDictionary>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(string dicKey,string lang)
        {
            string sql = "SELECT 1 FROM langDictionary  WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(string dicKey,string lang)
        {
            string sql = "SELECT 1 FROM langDictionary  WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(string dicKey,string lang)
        {
            string sql = "SELECT 1 FROM langDictionary  WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(string dicKey,string lang)
        {
            string sql = "SELECT 1 FROM langDictionary  WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(string dicKey,string lang, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM langDictionary  WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(string dicKey,string lang, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM langDictionary  WHERE 1=1  AND dicKey=@DicKey AND lang=@Lang LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@DicKey", value: dicKey, direction: ParameterDirection.Input);
			_params.Add("@Lang", value: lang, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [langDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [langDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [langDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [langDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [langDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [langDictionary] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM langDictionary ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM langDictionary ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM langDictionary ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM langDictionary ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM langDictionary WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "dicKey DicKey,lang Lang,dicValue DicValue";
        }
	}
}
