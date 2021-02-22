using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
	public partial interface IAppInfo : IDataBaseOperate<AppInfo>
	{
		bool Delete(int appId);        
        bool Delete(int appId, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int appId);
        Task<bool> DeleteAsync(int appId, IDbTransaction trans, IDbConnection conn);
        AppInfo GetModel(int appId);
        AppInfo GetModelByWriteDb(int appId);
        Task<AppInfo> GetModelAsync(int appId);
        Task<AppInfo> GetModelByWriteDbAsync(int appId);
        AppInfo GetModel(int appId, IDbTransaction trans, IDbConnection conn);
        Task<AppInfo> GetModelAsync(int appId, IDbTransaction trans, IDbConnection conn);
        bool Exists(int appId);
        bool ExistsByWriteDb(int appId);
        Task<bool> ExistsAsync(int appId);
        Task<bool> ExistsByWriteDbAsync(int appId);
        bool Exists(int appId, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int appId, IDbTransaction trans, IDbConnection conn);
		int GetLastIdentity();
	}
}
