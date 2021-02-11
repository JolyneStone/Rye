using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monica.DataAccess
{
    public interface ISecutiryPermissionService
    {
        public Task<IEnumerable<string>> GetPermissionCodeAsync(string roleIds);
    }
}
