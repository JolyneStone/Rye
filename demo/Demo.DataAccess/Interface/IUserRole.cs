using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Monica.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public interface IUserRole : IDataBaseOperate<UserRole>
	{
		bool Delete(int userId,int roleId);        
        bool Delete(int userId,int roleId, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int userId,int roleId);
        Task<bool> DeleteAsync(int userId,int roleId, IDbTransaction trans, IDbConnection conn);
        UserRole GetModel(int userId,int roleId);
        UserRole GetModelByWriteDb(int userId,int roleId);
        Task<UserRole> GetModelAsync(int userId,int roleId);
        Task<UserRole> GetModelByWriteDbAsync(int userId,int roleId);
        UserRole GetModel(int userId,int roleId, IDbTransaction trans, IDbConnection conn);
        Task<UserRole> GetModelAsync(int userId,int roleId, IDbTransaction trans, IDbConnection conn);
        bool Exists(int userId,int roleId);
        bool ExistsByWriteDb(int userId,int roleId);
        Task<bool> ExistsAsync(int userId,int roleId);
        Task<bool> ExistsByWriteDbAsync(int userId,int roleId);
        bool Exists(int userId,int roleId, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int userId,int roleId, IDbTransaction trans, IDbConnection conn);
	}
}
