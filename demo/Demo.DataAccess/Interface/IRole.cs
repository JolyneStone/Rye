using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Monica.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public interface IRole : IDataBaseOperate<Role>
	{
		bool Delete(int id);        
        bool Delete(int id, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id, IDbTransaction trans, IDbConnection conn);
        Role GetModel(int id);
        Role GetModelByWriteDb(int id);
        Task<Role> GetModelAsync(int id);
        Task<Role> GetModelByWriteDbAsync(int id);
        Role GetModel(int id, IDbTransaction trans, IDbConnection conn);
        Task<Role> GetModelAsync(int id, IDbTransaction trans, IDbConnection conn);
        bool Exists(int id);
        bool ExistsByWriteDb(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByWriteDbAsync(int id);
        bool Exists(int id, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int id, IDbTransaction trans, IDbConnection conn);
		int GetLastIdentity();
	}
}
