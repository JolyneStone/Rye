namespace KiraNet.AlasFx.Options
{
    /// <summary>
    /// 第三方OAuth2登录配置选项
    /// </summary>
    public class OAuth2Options
    {
        /// <summary>
        /// 本应用在第三方OAuth2系统中的客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 本应用在第三方OAuth2系统中的客户端密钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}