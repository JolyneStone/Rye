using System;
using System.Collections.Generic;

namespace Rye.Entities.Internal
{
    public class PermissionEntry<TPermissionKey>
         where TPermissionKey : IEquatable<TPermissionKey>
    {
        public TPermissionKey Id { get; set; }
        public TPermissionKey ParentId { get; set; }
        public string Code { get; set; }
        public List<PermissionEntry<TPermissionKey>> Children { get; set; }
    }
}
