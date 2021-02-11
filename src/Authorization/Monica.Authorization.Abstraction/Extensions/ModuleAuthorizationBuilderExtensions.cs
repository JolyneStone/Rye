using Microsoft.AspNetCore.Authorization;
using Monica.Authorization.Abstraction;
using Monica.Authorization.Abstraction.Builder;

namespace Monica
{
    public static class ModuleAuthorizationBuilderExtensions
    {
        public static void UseHandle<TAuthorizationHandler>(this IModuleAuthorizationBuilder builder)
            where TAuthorizationHandler : IAuthorizationHandler
        {
            builder.UseHandle(typeof(TAuthorizationHandler));
        }

        public static void UseMonicaHandle<TAuthorizationHandler>(this IModuleAuthorizationBuilder builder)
            where TAuthorizationHandler : MonicaPolicyAuthorizationHandler
        {
            builder.UseHandle(typeof(TAuthorizationHandler));
        }
    }
}
