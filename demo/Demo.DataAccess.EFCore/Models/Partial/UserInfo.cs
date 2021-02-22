using Rye.DataAccess;
using Rye.Entities;
using Rye.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class UserInfo : EntityBase<int>, IEntityUserBase<int>
    {
        public override int Key => Id;
    }
}
