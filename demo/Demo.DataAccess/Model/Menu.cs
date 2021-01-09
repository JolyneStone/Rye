using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class Menu
    {
		/// <summary>
		/// 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public int Sort { get; set; }
		/// <summary>
		/// 图标
		/// </summary>
		public string Icon { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public byte Status { get; set; }
		/// <summary>
		/// 父级菜单Id
		/// </summary>
		public int ParentId { get; set; }
		/// <summary>
		/// 应用Id
		/// </summary>
		public int AppId { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UpdateTime { get; set; }
    }
}