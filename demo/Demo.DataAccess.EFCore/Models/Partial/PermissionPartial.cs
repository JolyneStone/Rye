using Monica.DataAccess;
using Monica.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Permission : EntityBase<int>, IEntityPermissionBase<int>
    {
        public override int Key => Id;
    }
}
