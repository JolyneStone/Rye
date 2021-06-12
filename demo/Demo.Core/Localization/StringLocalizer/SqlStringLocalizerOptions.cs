using System;
using System.Data;

namespace Demo.Localization.StringLocalizer
{
    public class SqlStringLocalizerOptions
    {
        public Func<IDbConnection> CreateDbConnection { get; set; }
    }
}
