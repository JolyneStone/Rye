using Monica.DataAccess;
using Monica.Entities.Abstractions;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class AppInfo : EntityBase<int>, IEntityAppInfoBase<int>
    {
        public override int Key => AppId;
    }
}
