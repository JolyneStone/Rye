using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace Rye.Logger
{
    internal class SerilogStaticLog : IStaticLog
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
            if (logLevel == LogLevel.None)
                return;

            LogEventLevel level = logLevel switch
            {
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Critical => LogEventLevel.Fatal,
                LogLevel.Trace => LogEventLevel.Verbose,
                _ => LogEventLevel.Verbose
            };

            Serilog.Log.Write(level, message);
        }
    }
}
