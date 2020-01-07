using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx.Log
{
    public class AlasFxLogger : ILogger
    {
        public string CategoryName { get; }

        public AlasFxLogger(string categoryName)
        {
            CategoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return LogRecord.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!LogRecord.IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            LogRecord.Log(logLevel, CategoryName, formatter(state, exception));
        }

    }
}
