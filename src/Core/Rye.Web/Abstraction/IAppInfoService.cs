using Microsoft.Extensions.DependencyInjection;

namespace Rye.Web.Abstraction
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
    public interface IAppInfoService
    {
        public string GetAppKey();
        public string GetAppSecret(string appKey);
    }
}
