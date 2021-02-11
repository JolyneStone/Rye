using Demo.DataAccess.EFCore.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.EFCore.IRepository
{
    public interface IUserInfoRepository: Monica.DataAccess.IRepository<UserInfo, int>
    {
    }
}
