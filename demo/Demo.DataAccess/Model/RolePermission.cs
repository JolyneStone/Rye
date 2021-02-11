using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class RolePermission
    {
		/// <summary>
		/// 
		/// </summary>
		public int RoleId { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int PermissionId { get; set; }
    }
}