using Microsoft.Extensions.Logging;

using System;

namespace Monica.Logger
{
    public class MonicaLogger : ILogger
    {
        public string CategoryName { get; }

        public MonicaLogger(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName) && categoryName.IndexOf(".") >= 0)
            {
                var index = categoryName.LastIndexOf(".");
                if (index + 1 < categoryName.Length)
                {
                    categoryName = categoryName.Substring(index + 1);
                }
            }
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
