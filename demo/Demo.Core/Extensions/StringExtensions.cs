using Rye.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core
{
    public static class StringExtensions
    {
        public static string ToPassword(this string str)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return HashManager.GetMd5(HashManager.GetMd5(str + "Rye"));
        }
    }
}
