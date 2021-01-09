using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
        public sbyte Status { get; set; }
        public int ParentId { get; set; }
        public int AppId { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
