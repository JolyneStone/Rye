using Monica.DataAccess;
using Monica.Entities.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class RolePermission : EntityBase<string>, IEntityRolePermissionBase<int, int, string>
    {
        public override string Key => $"{RoleId}-{PermissionId}";
    }
}
