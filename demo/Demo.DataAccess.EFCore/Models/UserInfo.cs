using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string Nickame { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public sbyte Status { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public ulong Lock { get; set; }
        public DateTime? LockTime { get; set; }
        public string Password { get; set; }
        public int AppId { get; set; }
        public string ProfilePicture { get; set; }
    }
}
