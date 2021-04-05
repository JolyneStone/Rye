using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Entities.Abstractions
{
    public interface IAppInfoService
    {
        public string GetAppSecret(string appKey);
        Task<string> GetAppSecretAsync(string appKey);
    }
}
