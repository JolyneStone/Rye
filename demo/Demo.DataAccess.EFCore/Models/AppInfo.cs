using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class AppInfo
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
