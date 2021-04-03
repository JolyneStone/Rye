using Demo.Library.Dto;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Library.Abstraction
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
    public interface IUserPermissionService
    {
        Task<IEnumerable<PermissionTreeDto>> GetPermissionsTreeByRolesIdAsync(int[] rolesId);
        Task<IEnumerable<PermissionTreeDto>> GetPermissionsTreeByUserIdAsync(int userId);
    }
}
