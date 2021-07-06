using Rye.Entities;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class RolePermission : EntityBase<string>
    {
        public override string Key => $"{RoleId}-{PermissionId}";
    }
}
