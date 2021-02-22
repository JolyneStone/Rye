using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.Web.ResponseProvider.Security
{
    public class SecurityContext
    {
        public HttpContext HttpContext { get; set; }
    }
}
