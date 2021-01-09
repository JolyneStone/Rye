using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
