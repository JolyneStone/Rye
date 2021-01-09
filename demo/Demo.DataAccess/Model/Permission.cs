using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class Permission
    {
		/// <summary>
		/// 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 权限名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 权限状态
		/// </summary>
		public byte Status { get; set; }
		/// <summary>
		/// 菜单Id
		/// </summary>
		public int MenuId { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UpdateTime { get; set; }
    }
}