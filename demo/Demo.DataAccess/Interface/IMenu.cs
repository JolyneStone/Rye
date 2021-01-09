using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Monica.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public interface IMenu : IDataBaseOperate<Menu>
	{
		bool Delete(int id);        
        bool Delete(int id, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id, IDbTransaction trans, IDbConnection conn);
        Menu GetModel(int id);
        Menu GetModelByWriteDb(int id);
        Task<Menu> GetModelAsync(int id);
        Task<Menu> GetModelByWriteDbAsync(int id);
        Menu GetModel(int id, IDbTransaction trans, IDbConnection conn);
        Task<Menu> GetModelAsync(int id, IDbTransaction trans, IDbConnection conn);
        bool Exists(int id);
        bool ExistsByWriteDb(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByWriteDbAsync(int id);
        bool Exists(int id, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int id, IDbTransaction trans, IDbConnection conn);
		int GetLastIdentity();
	}
}
