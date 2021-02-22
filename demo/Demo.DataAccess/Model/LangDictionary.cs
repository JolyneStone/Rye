using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class LangDictionary
    {
		/// <summary>
		/// 
		/// </summary>
		public string DicKey { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Lang { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string DicValue { get; set; }
    }
}