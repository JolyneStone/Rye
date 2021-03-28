using Demo.Library.Dto;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Abstraction
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
    public interface IUserService
    {
        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserInfoDto> GetBasicUserAsync(int appId,int userId);
    }
}
