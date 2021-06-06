namespace Rye.Authorization
{
    public class TokenValidResult
    {
        /// <summary>
        /// 是否存在Token
        /// </summary>
        public bool HasToken { get; set; }
        /// <summary>
        /// Token验证是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Token是否过期
        /// </summary>
        public bool HasExpire { get; set; }
    }
}
