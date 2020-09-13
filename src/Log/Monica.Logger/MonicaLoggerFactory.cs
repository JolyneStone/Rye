using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monica.Logger
{
    public class MonicaLoggerFactory : ILoggerFactory
    {
        private readonly MonicaLoggerProvider _provider = new MonicaLoggerProvider();

        public void AddProvider(ILoggerProvider provider)
        {
            LogRecord.Debug(nameof(MonicaLoggerFactory), "AddProvider will be ignored");
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _provider.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _provider?.Dispose();
            }
        }
    }
}
