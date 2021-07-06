using System;
using System.Collections.Generic;

namespace Demo.Library.Dto
{
    public class PermissionEntryDto<TPermissionKey>
         where TPermissionKey : IEquatable<TPermissionKey>
    {
        public TPermissionKey Id { get; set; }
        public TPermissionKey ParentId { get; set; }
        public string Code { get; set; }
        public List<PermissionEntryDto<TPermissionKey>> Children { get; set; }
    }
}
