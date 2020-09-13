using System;

namespace Monica.DataAccess.Model
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
		public int IsPublish { get; set; }
		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime? PublishTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int BannerType { get; set; } = 0;
		/// <summary>
		/// 
		/// </summary>
		public int IsHot { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int IsRecommend { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Sort { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int RealReadNum { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int DefaultReadNum { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int RealCollectionNum { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int DefaultCollectionNum { get; set; }
		/// <summary>
		/// 新闻标题
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// 新闻摘要
		/// </summary>
		public string Summary { get; set; }
		/// <summary>
		/// 抓取来源地址
		/// </summary>
		public string LinkUrl { get; set; }
		/// <summary>
		/// 新闻标签/栏目
		/// </summary>
		public string Tags { get; set; }
		/// <summary>
		/// 封面图地址
		/// </summary>
		public string CoverImg { get; set; }
		/// <summary>
		/// Html格式的字符串
		/// </summary>
		public string Html { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int BannerSort { get; set; } = 0;
		/// <summary>
		/// 
		/// </summary>
		public int? NewsType { get; set; } = 0;
		/// <summary>
		/// 
		/// </summary>
		public int? SecondaryType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string ResourceUrl { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int ContractId { get; set; } = 0;
    }
}