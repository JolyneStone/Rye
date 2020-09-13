using Xunit;
using Monica.Logger;

using System;
using System.Collections.Generic;
using System.Text;
using Monica.Test;
using Monica.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monica.Options;
using Microsoft.Extensions.Options;

namespace Monica.Logger.Tests
{
    public class MonicaLoggerTests
    {
        public MonicaLoggerTests()
        {
            TestSetup.ConfigService();
        }

        [Fact()]
        public void LogTest()
        {
            var loggerFactory = SingleServiceLocator.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<MonicaLoggerTests>();
            logger.LogDebug("test");
            logger.LogInformation("test");
        }
    }
}