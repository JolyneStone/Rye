using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rye.Logger;

namespace Rye
{
    public sealed class Log
    {
        public static IStaticLog Current { get; internal set; }
    }

}
