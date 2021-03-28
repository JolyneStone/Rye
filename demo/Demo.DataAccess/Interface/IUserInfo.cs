using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public partial interface IUserInfo : IDataBaseOperate<UserInfo>
	{
		bool Delete(int id);        
        bool Delete(int id, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id, IDbTransaction trans, IDbConnection conn);
        UserInfo GetModel(int id);
        UserInfo GetModelByWriteDb(int id);
        Task<UserInfo> GetModelAsync(int id);
        Task<UserInfo> GetModelByWriteDbAsync(int id);
        UserInfo GetModel(int id, IDbTransaction trans, IDbConnection conn);
        Task<UserInfo> GetModelAsync(int id, IDbTransaction trans, IDbConnection conn);
        bool Exists(int id);
        bool ExistsByWriteDb(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByWriteDbAsync(int id);
        bool Exists(int id, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int id, IDbTransaction trans, IDbConnection conn);
		int GetLastIdentity();
	}
}
