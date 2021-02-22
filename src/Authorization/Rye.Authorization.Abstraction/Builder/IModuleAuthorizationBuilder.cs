using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Authorization.Abstraction.Builder
{
    public interface IModuleAuthorizationBuilder
    {
        public Action<AuthorizationOptions> ConfigureOptions { get; set; }
        public void UseHandle(Type AuthorizationHandlerType);
    }
}
