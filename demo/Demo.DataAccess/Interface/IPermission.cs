using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Monica.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public interface IPermission : IDataBaseOperate<Permission>
	{
		bool Delete(int id);        
        bool Delete(int id, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id, IDbTransaction trans, IDbConnection conn);
        Permission GetModel(int id);
        Permission GetModelByWriteDb(int id);
        Task<Permission> GetModelAsync(int id);
        Task<Permission> GetModelByWriteDbAsync(int id);
        Permission GetModel(int id, IDbTransaction trans, IDbConnection conn);
        Task<Permission> GetModelAsync(int id, IDbTransaction trans, IDbConnection conn);
        bool Exists(int id);
        bool ExistsByWriteDb(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByWriteDbAsync(int id);
        bool Exists(int id, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int id, IDbTransaction trans, IDbConnection conn);
		int GetLastIdentity();
	}
}
