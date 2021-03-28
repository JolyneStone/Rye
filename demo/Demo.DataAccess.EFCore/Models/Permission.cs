using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Permission
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentId { get; set; }
        public sbyte Status { get; set; }
        public sbyte Type { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
