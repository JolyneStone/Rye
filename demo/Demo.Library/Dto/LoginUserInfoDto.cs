using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Dto
{
    public class LoginUserInfoDto
    {
        public int Id { get; set; }
        public string Nickame { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int AppId { get; set; }
        public int[] RoleIds { get; set; }
    }
}
