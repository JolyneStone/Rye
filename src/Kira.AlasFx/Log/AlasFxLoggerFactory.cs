using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx.Log
{
    public class AlasFxLoggerFactory : ILoggerFactory
    {
        private readonly AlasFxLoggerProvider _provider = new AlasFxLoggerProvider();

        public void AddProvider(ILoggerProvider provider)
        {
            LogRecord.Debug(nameof(AlasFxLoggerFactory), "AddProvider will be ignored");
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
