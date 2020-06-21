using Xunit;
using Raven.Log;

using System;
using System.Collections.Generic;
using System.Text;
using Raven.Test;
using Raven.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raven.Options;
using Microsoft.Extensions.Options;

namespace Raven.Log.Tests
{
    public class RavenLoggerTests
    {
        public RavenLoggerTests()
        {
            TestSetup.ConfigService();
        }

        [Fact()]
        public void LogTest()
        {
            var loggerFactory = SingleServiceLocator.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<RavenLoggerTests>();
            logger.LogDebug("test");
            logger.LogInformation("test");
        }
    }
}