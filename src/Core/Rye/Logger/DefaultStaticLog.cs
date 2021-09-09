using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rye.Logger
{
    internal class DefaultStaticLog : IStaticLog
    {
        public void Write(LogLevel logLevel, string fileName, string message)
        {
            LogRecord.Log(logLevel, fileName, message);
        }
    }
}
