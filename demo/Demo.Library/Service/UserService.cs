using Demo.DataAccess.EFCore;
using Demo.DataAccess.EFCore.Models;
using Demo.Library.Abstraction;
using Demo.Library.Dto;

using Rye;
using Rye.EntityFrameworkCore;
using Rye.Enums;
using Rye.Security;

using System.Threading.Tasks;

namespace Demo.Library.Service
{
    public class UserService : IUserService
    {
        private readonly IDbProvider _provider;
        private readonly ISecurityService _securityService;

        public UserService(IDbProvider provider, ISecurityService securityService)
        {
            _provider = provider;
            _securityService = securityService;
        }

        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfoDto> GetBasicUserAsync(int appId, int userId)
        {
            var uow = _provider.GetUnitOfWorkByReadDb();
            var userInfoRepository = uow.GetReadOnlyRepository<UserInfo, int>();

            var user = await userInfoRepository.GetFirstOrDefaultAsync(d =>
                d.Id == userId &&
                d.Status == (sbyte)EntityStatus.Enabled);

            return user == null ?
                null :
                new UserInfoDto
                {
                    Id = user.Id,
                    Nickname = user.Nickame,
                    Avatar = App.Configuration.GetSectionValue("Domain") + user.ProfilePicture,
                    Phone = user.Phone.IsNullOrEmpty() ? "" : await _securityService.DecryptAsync(appId, user.Phone),
                    Email = user.Email.IsNullOrEmpty() ? "" : await _securityService.DecryptAsync(appId, user.Email),
                };
        }
    }
}
