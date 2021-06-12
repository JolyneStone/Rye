using Demo.Common.Enums;
using Demo.Library.Dto;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Demo.Library.Abstraction
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
    public interface ILoginService
    {
        Task<(DefaultStatusCode, LoginUserInfoDto)> LoginAsync(string appKey, string mobile, string password);
    }
}
