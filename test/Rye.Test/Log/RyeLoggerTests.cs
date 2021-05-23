using Xunit;
using Rye.Logger;

using System;
using System.Collections.Generic;
using System.Text;
using Rye.Test;
using Rye.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rye.Options;
using Microsoft.Extensions.Options;

namespace Rye.Logger.Tests
{
    public class RyeLoggerTests
    {
        public RyeLoggerTests()
        {
            TestSetup.ConfigService(serviceCollection=>serviceCollection.AddRye().ConfigureModule());
        }

        [Fact()]
        public void LogTest()
        {
            var loggerFactory = App.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<RyeLoggerTests>();
            logger.LogDebug("test");
            logger.LogInformation("test");
        }
    }
}