using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class AppInfo
    {
		/// <summary>
		/// 
		/// </summary>
		public int AppId { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Remark { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string AppKey { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string AppSecret { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public byte Status { get; set; }
    }
}