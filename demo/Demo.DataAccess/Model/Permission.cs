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
		/// 应用Id
		/// </summary>
		public int AppId { get; set; }
		/// <summary>
		/// 权限名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 权限编码
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// 父级权限Id
		/// </summary>
		public int ParentId { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public byte Status { get; set; }
		/// <summary>
		/// 权限类型
		/// </summary>
		public byte Type { get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public int Sort { get; set; }
		/// <summary>
		/// 图标
		/// </summary>
		public string Icon { get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		public string Link { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}