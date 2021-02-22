using Rye.DataAccess;
using Rye.Entities;
using Rye.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Permission : EntityBase<int>, IEntityPermissionBase<int>
    {
        public override int Key => Id;
    }
}
