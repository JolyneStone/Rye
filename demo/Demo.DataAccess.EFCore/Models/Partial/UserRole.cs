using Rye.DataAccess;
using Rye.Entities;
using Rye.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class UserRole : EntityBase<string>, IEntityUserRoleBase<int, int, string>
    {
        public override string Key => $"{UserId}-{RoleId}";
    }
}
