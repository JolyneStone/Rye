using Microsoft.Extensions.Logging;
using System;

namespace Rye.Logger
{
    public class RyeLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new RyeLogger(categoryName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}