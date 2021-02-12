using Monica.Enums;

namespace Monica.Jwt.Entities
{
    public class TokenEntityBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        public string ClientType { get; set; }
    }
}
