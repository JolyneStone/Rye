using Rye.DataAccess;
using Rye.Entities;
using Rye.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class AppInfo : EntityBase<int>, IEntityAppInfoBase<int>
    {
        public override int Key => AppId;
    }
}
