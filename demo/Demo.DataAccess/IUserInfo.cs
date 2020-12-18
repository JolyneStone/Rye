using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Monica.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public interface IUserInfo : IDataBaseOperate<UserInfo>
	{
		bool Delete(long id);        
        bool Delete(long id, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteAsync(long id, IDbTransaction trans, IDbConnection conn);
        UserInfo GetModel(long id);
        UserInfo GetModelByWriteDb(long id);
        Task<UserInfo> GetModelAsync(long id);
        Task<UserInfo> GetModelByWriteDbAsync(long id);
        UserInfo GetModel(long id, IDbTransaction trans, IDbConnection conn);
        Task<UserInfo> GetModelAsync(long id, IDbTransaction trans, IDbConnection conn);
        bool Exists(long id);
        bool ExistsByWriteDb(long id);
        Task<bool> ExistsAsync(long id);
        Task<bool> ExistsByWriteDbAsync(long id);
        bool Exists(long id, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(long id, IDbTransaction trans, IDbConnection conn);
		long GetLastIdentity();
	}
}
