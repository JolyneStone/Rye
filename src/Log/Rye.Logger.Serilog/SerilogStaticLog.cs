using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace Rye.Logger
{
    internal class SerilogStaticLog : IStaticLog
    {
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
