using Rye.Entities;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class UserRole : EntityBase<string>
    {
        public override string Key => $"{UserId}-{RoleId}";
    }
}
