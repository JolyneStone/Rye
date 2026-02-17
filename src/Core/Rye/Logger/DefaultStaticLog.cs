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
        public void None(string message)
        {
            Write(LogLevel.None, null, message);
        }

        public void Debug(string message)
        {
            Write(LogLevel.Debug, null, message);
        }

        public void Trace(string message)
        {
            Write(LogLevel.Trace, null, message);
        }

        public void Warning(string message)
        {
            Write(LogLevel.Warning, null, message);
        }
        
        public void Information(string message)
        {
            Write(LogLevel.Information, null, message);
        }

        public void Error(string message)
        {
            Write(LogLevel.Error, null, message);
        }

        public void Critical(string message)
        {
            Write(LogLevel.Critical, null, message);
        }

        public void Write(LogLevel logLevel, string fileName, string message)
        {
            if (fileName.IsNullOrEmpty())
            {
                fileName = DateTime.Now.ToString("yyyyMMddHH")+".log";
            }
            LogRecord.Log(logLevel, fileName, message);
        }
    }
}
