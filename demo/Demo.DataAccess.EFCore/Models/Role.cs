using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public sbyte Status { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
