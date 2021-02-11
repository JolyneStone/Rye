using Monica.DataAccess;
using Monica.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class UserInfo : EntityBase<int>, IEntityUserBase<int>
    {
        public override int Key => Id;
    }
}
