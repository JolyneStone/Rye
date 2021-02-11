using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Monica.Authorization.Abstraction.Builder
{
    public class ModuleAuthorizationBuilder : IModuleAuthorizationBuilder
    {
        private readonly IServiceCollection _serviceCollection;
        public ModuleAuthorizationBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public Action<AuthorizationOptions> ConfigureOptions { get; set; }

        public void UseHandle(Type AuthorizationHandlerType)
        {
            _serviceCollection.AddSingleton(typeof(IAuthorizationHandler), AuthorizationHandlerType);
        }
    }
}
