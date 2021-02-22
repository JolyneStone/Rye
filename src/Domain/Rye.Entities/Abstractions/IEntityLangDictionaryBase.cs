namespace Rye.Entities.Abstractions
{
    public interface IEntityLangDictionaryBase : IEntity<string>
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
