using Microsoft.Extensions.Logging;
using System;

namespace Monica.Logger
{
    public class MonicaLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MonicaLogger(categoryName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}