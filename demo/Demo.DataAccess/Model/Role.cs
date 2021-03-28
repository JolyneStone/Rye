using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class Role
    {
		/// <summary>
		/// 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// App Id
		/// </summary>
		public int AppId { get; set; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public byte Status { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remarks { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }
    }
}