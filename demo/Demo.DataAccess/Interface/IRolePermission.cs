using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
	public partial interface IRolePermission : IDataBaseOperate<RolePermission>
	{
		bool Delete(int roleId,int permissionId);        
        bool Delete(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(int roleId,int permissionId);
        Task<bool> DeleteAsync(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn);
        RolePermission GetModel(int roleId,int permissionId);
        RolePermission GetModelByWriteDb(int roleId,int permissionId);
        Task<RolePermission> GetModelAsync(int roleId,int permissionId);
        Task<RolePermission> GetModelByWriteDbAsync(int roleId,int permissionId);
        RolePermission GetModel(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn);
        Task<RolePermission> GetModelAsync(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn);
        bool Exists(int roleId,int permissionId);
        bool ExistsByWriteDb(int roleId,int permissionId);
        Task<bool> ExistsAsync(int roleId,int permissionId);
        Task<bool> ExistsByWriteDbAsync(int roleId,int permissionId);
        bool Exists(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(int roleId,int permissionId, IDbTransaction trans, IDbConnection conn);
	}
}
