using Microsoft.Extensions.Logging;

namespace Rye.Logger
{
    public interface IStaticLog
    {
        void Write(LogLevel logLevel, string fileName, string message);

        public void None(string fileName, string message)
        {
            Write(LogLevel.None, fileName, message);
        }

        public void Trace(string fileName, string message)
        {
            Write(LogLevel.Trace, fileName, message);
        }

        public void Debug(string fileName, string message)
        {
            Write(LogLevel.Debug, fileName, message);
        }

        public void Information(string fileName, string message)
        {
            Write(LogLevel.Information, fileName, message);
        }

        public void Warning(string fileName, string message)
        {
            Write(LogLevel.Warning, fileName, message);
        }

        public void Error(string fileName, string message)
        {
            Write(LogLevel.Error, fileName, message);
        }

        public void Critical(string fileName, string message)
        {
            Write(LogLevel.Critical, fileName, message);
        }
    }
}
