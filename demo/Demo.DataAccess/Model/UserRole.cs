using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class UserRole
    {
		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserId { get; set; }
		/// <summary>
		/// 角色Id
		/// </summary>
		public int RoleId { get; set; }
    }
}