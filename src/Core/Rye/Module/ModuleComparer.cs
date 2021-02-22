using System.Collections.Generic;

namespace Rye.Module
{
    public class ModuleComparer : IComparer<IStartupModule>
    {
        public int Compare(IStartupModule x, IStartupModule y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(x, null)) return 1;
            if (ReferenceEquals(y, null)) return -1;
            if (x.GetType() == y.GetType()) return 0;
            if (x.Level < y.Level) return -1;
            if (x.Level > y.Level) return 1;
            if (x.Order < y.Order) return -1;
            if (x.Order > y.Order) return 1;
            return string.Compare(x.GetType().FullName, y.GetType().FullName);
        }
    }
}
