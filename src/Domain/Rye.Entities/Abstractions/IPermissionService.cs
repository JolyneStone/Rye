using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.Entities.Abstractions
{
    public interface IPermissionService
    {
        public Task<IEnumerable<string>> GetPermissionCodeAsync(string roleIds);
    }
}
