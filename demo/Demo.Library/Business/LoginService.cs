using Demo.Core.Common.Enums;
using Demo.DataAccess.EFCore;
using Demo.DataAccess.EFCore.Models;
using Demo.Library.Abstraction;
using Demo.Library.Dto;

using Rye.EntityFrameworkCore;
using Rye.Enums;
using Rye.Security;
using Mapster;
using System;
using System.Threading.Tasks;
using Rye.Util;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Demo.Core;

namespace Demo.Library.Business
{
    public class LoginService : ILoginService
    {
        private readonly IDbProvider _provider;
        private readonly ISecurityService _securityService;

        public LoginService(IDbProvider provider, ISecurityService securityService)
        {
            _provider = provider;
            _securityService = securityService;
        }

        public async Task<(CommonStatusCode, LoginUserInfoDto)> LoginAsync(string appKey, string account, string password)
        {
            var uow = _provider.GetUnitOfWorkByReadDb();
            var userInfoRepository = uow.GetReadOnlyRepository<UserInfo, int>();
            var appInfoRepository = uow.GetReadOnlyRepository<AppInfo, int>();
            var appInfo = await appInfoRepository.GetFirstOrDefaultAsync(d => d.AppKey == appKey);
            if (appInfo == null)
            {
                return (CommonStatusCode.AppNotExist, null);
            }
            UserInfo userInfo;
            if (StringUtil.IsMobile(account))
            {
                var mobile = _securityService.Encrypt(appInfo.AppId, account);
                userInfo = await userInfoRepository.GetFirstOrDefaultAsync(d =>
                    d.AppId == appInfo.AppId &&
                    d.Phone == mobile &&
                    d.Status == (sbyte)EntityStatus.Enabled
                );
            }
            else if (StringUtil.IsEmail(account))
            {
                var email = _securityService.Encrypt(appInfo.AppId, account);
                userInfo = await userInfoRepository.GetFirstOrDefaultAsync(d =>
                    d.AppId == appInfo.AppId &&
                    d.Email == email &&
                    d.Status == (sbyte)EntityStatus.Enabled
                );
            }
            else
            {
                return (CommonStatusCode.InvalidAccount, null);
            }

            if (userInfo == null)
            {
                return (CommonStatusCode.AccountError, null);
            }

            if (userInfo.LockTime.HasValue && userInfo.LockTime.Value < DateTime.UtcNow)
            {
                return (CommonStatusCode.AccountAlreadyLock, null);
            }

            if (userInfo.Password != password.ToPassword())
            {
                userInfo.Lock++;
                uow = _provider.GetUnitOfWorkByWriteDb();
                var repo = uow.GetRepository<UserInfo, int>();
                if (userInfo.Lock < 5)
                {
                    await repo.UpdateAsync(userInfo);
                    await uow.SaveChangesAsync();
                    return (CommonStatusCode.AccountError, null);
                }
                else
                {
                    userInfo.LockTime = DateTime.UtcNow.AddHours(2);
                    await repo.UpdateAsync(userInfo);
                    await uow.SaveChangesAsync();
                    return (CommonStatusCode.AccountError, null);
                }
            }

            var userRoleRepository = uow.GetReadOnlyRepository<UserRole, string>();
            var roleIds = await userRoleRepository.Query(d => d.UserId == userInfo.Id).Select(d => d.RoleId).ToArrayAsync();
            var rolesRepository = uow.GetReadOnlyRepository<Role, int>();
            roleIds = await rolesRepository.Query(d => d.AppId == appInfo.AppId && roleIds.Contains(d.Id)).Select(d => d.Id).ToArrayAsync();

            return (CommonStatusCode.Success, new LoginUserInfoDto
            {
                Id = userInfo.Id,
                AppId = userInfo.AppId,
                Nickame = userInfo.Nickame,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                RoleIds = roleIds
            });
        }
    }
}
