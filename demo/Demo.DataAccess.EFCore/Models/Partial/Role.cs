using Rye.DataAccess;
using Rye.Entities;
using Rye.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Role : EntityBase<int>, IEntityRoleBase<int>
    {
        public override int Key => Id;
    }
}
