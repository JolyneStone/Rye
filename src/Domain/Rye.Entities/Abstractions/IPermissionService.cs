using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.Entities.Abstractions
{
    public interface IPermissionService<TPermissionKey>
        where TPermissionKey : IEquatable<TPermissionKey>
    {
        public Task<IEnumerable<string>> GetPermissionCodeAsync(string roleIds);
    }
}
