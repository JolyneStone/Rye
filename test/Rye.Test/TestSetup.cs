using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.IO;

namespace Rye.Test
{
    public class TestSetup
    {
        public static IServiceProvider ConfigService(Action<IServiceCollection> configAction)
        {
            var builder = Host.CreateApplicationBuilder()
               .ConfigureApp();

            configAction(builder.Services);

            App.ConfigureServiceLocator(builder.Services.BuildServiceProvider());
            return App.ApplicationServices;
        }
    }
}
