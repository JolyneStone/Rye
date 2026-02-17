using Microsoft.Extensions.Logging;
using System;

namespace Rye.Logger
{
    public interface IStaticLog
    {
        void Write(LogLevel logLevel, string fileName, string message);

        public void None(string fileName, string message)
        {
            Write(LogLevel.None, fileName, message);
        }

        void None(string message);

        public void Trace(string fileName, string message)
        {
            Write(LogLevel.Trace, fileName, message);
        }

        void Trace(string message);

        public void Debug(string fileName, string message)
        {
            Write(LogLevel.Debug, fileName, message);
        }

        void Debug(string message);

        public void Information(string fileName, string message)
        {
            Write(LogLevel.Information, fileName, message);
        }

        void Information(string message);

        public void Warning(string fileName, string message)
        {
            Write(LogLevel.Warning, fileName, message);
        }

        void Warning(string message);

        public void Error(string fileName, string message)
        {
            Write(LogLevel.Error, fileName, message);
        }

        void Error(string message);

        public void Critical(string fileName, string message)
        {
            Write(LogLevel.Critical, fileName, message);
        }

        void Critical(string message);
    }
}
