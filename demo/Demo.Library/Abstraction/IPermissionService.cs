using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Library.Abstraction
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
    public interface IPermissionService
    {
        public Task<IEnumerable<string>> GetPermissionCodeAsync(string roleIds);
    }
}
