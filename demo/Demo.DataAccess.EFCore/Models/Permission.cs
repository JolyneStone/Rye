using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public sbyte Status { get; set; }
        public int MenuId { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
