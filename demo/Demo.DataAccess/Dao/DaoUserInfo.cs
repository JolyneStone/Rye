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
	public partial class DaoUserInfo : IUserInfo
    {
        public MySqlConnectionProvider ConnectionProvider { get; }

        public DaoUserInfo(MySqlConnectionProvider provider)
        {
            ConnectionProvider = provider;
        }

		public long GetLastIdentity()
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
			return conn.ExecuteScalar<long>("SELECT SCOPE_IDENTITY()");
		}

        public int Insert(UserInfo model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture) VALUES (@Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture);";

            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> InsertAsync(UserInfo model, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture) VALUES (@Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture);";

            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int Insert(UserInfo model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Insert(model, null, conn);
        }

        public async Task<int> InsertAsync(UserInfo model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertAsync(model, null, conn);
        }

        public int BatchInsert(IEnumerable<UserInfo> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture) VALUES (@Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture);";

            if (trans == null)
                return conn.Execute(sql, param: items, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<UserInfo> items, IDbTransaction trans, IDbConnection conn)
        {
        	string sql = "INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture) VALUES (@Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture);";

             if (trans == null)
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text, transaction: trans);
        }

        public int BatchInsert(IEnumerable<UserInfo> items)
        {
        	string sql = "INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture) VALUES (@Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Execute(sql, param: items, commandType: CommandType.Text);
        }
        
        public async Task<int> BatchInsertAsync(IEnumerable<UserInfo> items)
        {
        	string sql = "INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture) VALUES (@Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture);";

            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteAsync(sql, param: items, commandType: CommandType.Text);
        }

        public int InsertUpdate(UserInfo model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE userInfo SET  nickame=@Nickame, phome=@Phome, email=@Email, status=@Status, registerTime=@RegisterTime, updateTime=@UpdateTime, lock=@Lock, lockTime=@LockTime, password=@Password, appId=@AppId, profilePicture=@ProfilePicture WHERE 1=1  AND id=@Id;INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture); SELECT @Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture WHERE NOT EXISTS (SELECT 1 FROM userInfo where 1=1  AND id=@Id)";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }
        
        public async Task<int> InsertUpdateAsync(UserInfo model, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "UPDATE userInfo SET  nickame=@Nickame, phome=@Phome, email=@Email, status=@Status, registerTime=@RegisterTime, updateTime=@UpdateTime, lock=@Lock, lockTime=@LockTime, password=@Password, appId=@AppId, profilePicture=@ProfilePicture WHERE 1=1  AND id=@Id;INSERT INTO userInfo (nickame,phome,email,status,registerTime,updateTime,lock,lockTime,password,appId,profilePicture); SELECT @Nickame,@Phome,@Email,@Status,@RegisterTime,@UpdateTime,@Lock,@LockTime,@Password,@AppId,@ProfilePicture WHERE NOT EXISTS (SELECT 1 FROM userInfo where 1=1  AND id=@Id)";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
        }

        public int InsertUpdate(UserInfo model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            {
                return InsertUpdate(model, null, conn);
            }
        }
        
        public async Task<int> InsertUpdateAsync(UserInfo model)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await InsertUpdateAsync(model, null, conn);
        }
        
        public int Update(UserInfo model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "UPDATE userInfo SET  nickame=@Nickame, phome=@Phome, email=@Email, status=@Status, registerTime=@RegisterTime, updateTime=@UpdateTime, lock=@Lock, lockTime=@LockTime, password=@Password, appId=@AppId, profilePicture=@ProfilePicture WHERE 1=1  AND id=@Id";
            if (trans == null)
                return conn.Execute(sql, param: model, commandType: CommandType.Text);
            else
                return conn.Execute(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public int Update(UserInfo model)
		{
			IDbConnection conn = ConnectionProvider.GetConnection();
            return Update(model, null, conn);
		}
        
        public async Task<int> UpdateAsync(UserInfo model, IDbTransaction trans, IDbConnection conn)
		{
            string sql = "GetUpdateSql()";
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text);
            else
                return await conn.ExecuteAsync(sql, param: model, commandType: CommandType.Text, transaction: trans);
		}
        
        public async Task<int> UpdateAsync(UserInfo model)
		{
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await UpdateAsync(model, null, conn);
        }

        public bool Delete(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            if (trans == null)
                return conn.Execute(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.Execute(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public bool Delete(long id)
        {
            IDbConnection conn = ConnectionProvider.GetConnection();
            return Delete(id, null,conn);
        }

        public async Task<bool> DeleteAsync(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "DELETE FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            if (trans == null)
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteAsync(sql, param: _params, commandType: CommandType.Text,transaction: trans) > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {   
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await DeleteAsync(id, null,conn);
        }

        public UserInfo GetModel(long id)
		{
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<UserInfo>(sql, param: _params, commandType: CommandType.Text);
		}				
        
        public UserInfo GetModelByWriteDb(long id)
		{
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<UserInfo>(sql, param: _params, commandType: CommandType.Text);
		}
        
        public async Task<UserInfo> GetModelAsync(long id)
		{  
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: _params, commandType: CommandType.Text);
		}	
        
        public async Task<UserInfo> GetModelByWriteDbAsync(long id)
		{  
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
    		IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: _params, commandType: CommandType.Text);
		}	

        public UserInfo GetModel(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return conn.QueryFirstOrDefault<UserInfo>(sql, param: _params, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<UserInfo>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<UserInfo> GetModelAsync(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo WHERE 1=1 AND id=@Id";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: _params, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: _params, commandType: CommandType.Text, transaction: trans);

        }

        public UserInfo GetModel(object param, string whereSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserInfo> GetModelAsync(object param, string whereSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public UserInfo GetModelByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserInfo> GetModelByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public UserInfo GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<UserInfo> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public UserInfo FirstOrDefault(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserInfo> FirstOrDefaultAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public UserInfo FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<UserInfo> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public UserInfo FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.QueryFirstOrDefault<UserInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<UserInfo> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo LIMIT 1 WHERE 1=1 AND " + whereSql + "ORDER BY " + orderSql + " LIMIT 1";
            
            if (trans == null)
                return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }
		
        public IEnumerable<UserInfo> GetList()
        {
            string sql = "GetSelectAllSql()";
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<UserInfo>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserInfo>> GetListAsync()
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  ORDER BY id DESC";
            
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<UserInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserInfo> GetList(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "GetSelectAllSql()";
                
            if (trans == null)
                return conn.Query<UserInfo>(sql, commandType: CommandType.Text);

            else
                return conn.Query<UserInfo>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<UserInfo>> GetListAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  ORDER BY id DESC";
                
            return await conn.QueryAsync<UserInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserInfo> GetListByWriteDb()
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  ORDER BY id DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<UserInfo>(sql, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserInfo>> GetListByWriteDbAsync()
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  ORDER BY id DESC";
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<UserInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserInfo> GetListByWriteDb(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  ORDER BY id DESC";
                
            if (trans == null)
                return conn.Query<UserInfo>(sql, commandType: CommandType.Text);

            else
                return conn.Query<UserInfo>(sql, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<UserInfo>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  ORDER BY id DESC";
                
            return await conn.QueryAsync<UserInfo>(sql, commandType: CommandType.Text);
        }

        public IEnumerable<UserInfo> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.Query<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserInfo>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.QueryAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<UserInfo> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.Query<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<UserInfo>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.QueryAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);
        }

        public IEnumerable<UserInfo> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return conn.Query<UserInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return conn.Query<UserInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public async Task<IEnumerable<UserInfo>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn)
        {
            string sql = string.Format("SELECT id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture FROM userInfo  WHERE {0} ORDER BY {1} LIMIT {3},{2}", whereSql, orderSql, (pageIndex - 1) * pageSize, pageSize);
                
            if (trans == null)
                return await conn.QueryAsync<UserInfo>(sql, param: param, commandType: CommandType.Text);

            else
                return await conn.QueryAsync<UserInfo>(sql, param: param, commandType: CommandType.Text, transaction: trans);

        }

        public bool Exists(long id)
        {
            string sql = "SELECT 1 FROM userInfo  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(long id)
        {
            string sql = "SELECT 1 FROM userInfo  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            string sql = "SELECT 1 FROM userInfo  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(long id)
        {
            string sql = "SELECT 1 FROM userInfo  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
        }

        public bool Exists(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM userInfo  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(long id, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM userInfo  WHERE 1=1  AND id=@Id LIMIT 1";
            var _params = new DynamicParameters();
			_params.Add("@Id", value: id, direction: ParameterDirection.Input);
                
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: _params, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public bool Exists(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool ExistsByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public async Task<bool> ExistsByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT 1 FROM [userInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
        }

        public bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [userInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public async Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT 1 FROM [userInfo] WITH(NOLOCK) WHERE 1=1 AND " + whereSql + " LIMIT 1";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text) > 0;
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans) > 0;
        }

        public int Count()
        {
            string sql = "SELECT COUNT(1) FROM userInfo ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync()
        {
            string sql = "SELECT COUNT(1) FROM userInfo ";
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int CountByWriteDb()
        {
            string sql = "SELECT COUNT(1) FROM userInfo ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
        }

        public async Task<int> CountByWriteDbAsync()
        {
            string sql = "SELECT COUNT(1) FROM userInfo ";
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
        }

        public int Count(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }

        public async Task<int> CountAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetReadOnlyConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int CountByWriteDb(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
        }
        
        public async Task<int> CountByWriteDbAsync(object param, string whereSql)
        {
            string sql = "SELECT COUNT(1) FROM userInfo  WHERE 1=1 AND " + whereSql;
            IDbConnection conn = ConnectionProvider.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
        }

        public int Count(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userInfo ";
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userInfo ";
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text, transaction: trans);
        }

        public int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userInfo WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text);
            else
                return conn.ExecuteScalar<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }

        public async Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn)
        {
            string sql = "SELECT COUNT(1) FROM userInfo WHERE 1=1 AND " + whereSql;
            if (trans == null)
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text);
            else
                return await conn.ExecuteScalarAsync<int>(sql, param: param, commandType: CommandType.Text, transaction: trans);
        }
        
        
        private string GetColumns()
        {
            return "id Id,nickame Nickame,phome Phome,email Email,status Status,registerTime RegisterTime,updateTime UpdateTime,lock Lock,lockTime LockTime,password Password,appId AppId,profilePicture ProfilePicture";
        }
	}
}
