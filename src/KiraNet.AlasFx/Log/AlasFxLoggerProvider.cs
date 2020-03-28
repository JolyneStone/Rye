using Microsoft.Extensions.Logging;
using System;

namespace KiraNet.AlasFx.Log
{
    public class AlasFxLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new AlasFxLogger(categoryName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}