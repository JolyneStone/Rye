using Microsoft.AspNetCore.Authorization;
using Rye.Authorization.Abstraction;
using Rye.Authorization.Abstraction.Builder;

namespace Rye
{
    public static class ModuleAuthorizationBuilderExtensions
    {
        public static void UseHandle<TAuthorizationHandler>(this IModuleAuthorizationBuilder builder)
            where TAuthorizationHandler : IAuthorizationHandler
        {
            builder.UseHandle(typeof(TAuthorizationHandler));
        }

        public static void UseRyeHandle<TAuthorizationHandler>(this IModuleAuthorizationBuilder builder)
            where TAuthorizationHandler : RyePolicyAuthorizationHandler
        {
            builder.UseHandle(typeof(TAuthorizationHandler));
        }
    }
}
