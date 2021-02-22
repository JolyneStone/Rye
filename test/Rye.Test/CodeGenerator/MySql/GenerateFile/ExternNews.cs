using System;

namespace Rye.DataAccess.MySql.Model
{
    [Serializable]
    public partial class ExternNews
    {
		/// <summary>
		/// 
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int SourceType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long SourceId { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? IsPublish { get; set; }
		/// <summary>
		/// title
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int ContractId { get; set; } = 0;
    }
}