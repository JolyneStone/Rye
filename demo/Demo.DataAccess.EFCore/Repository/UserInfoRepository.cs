using Demo.DataAccess.EFCore.IRepository;
using Demo.DataAccess.EFCore.Models;
using Monica.EntityFrameworkCore;

namespace Demo.DataAccess.EFCore.Repository
{
    public class UserInfoRepository : Monica.EntityFrameworkCore.Repository<UserInfo, int>, IUserInfoRepository
    {
        public UserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
