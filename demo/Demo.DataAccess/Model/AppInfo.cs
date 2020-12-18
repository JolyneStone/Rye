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
		public string Appkey { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime { get; set; }
    }
}