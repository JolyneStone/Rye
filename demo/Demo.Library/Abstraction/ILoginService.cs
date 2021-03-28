using Demo.Core.Common.Enums;
using Demo.Library.Dto;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Demo.Library.Abstraction
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
    public interface ILoginService
    {
        Task<(CommonStatusCode, LoginUserInfoDto)> LoginAsync(string appKey, string mobile, string password);
    }
}
