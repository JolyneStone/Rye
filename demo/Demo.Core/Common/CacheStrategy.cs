namespace Demo.Common
{
    /// <summary>
    /// 缓存过期时间策略
    /// </summary>
    public static class CacheStrategy
    {
        /// <summary>
        /// 一个月30天过期
        /// </summary>
        public const int ONE_MONTH = 2592000;

        /// <summary>
        /// 一天过期24小时
        /// </summary>

        public const int ONE_DAY = 86400;

        /// <summary>
        /// 12小时过期
        /// </summary>

        public const int HALF_DAY = 43200;

        /// <summary>
        /// 8小时过期
        /// </summary>

        public const int EIGHT_HOURS = 28800;

        /// <summary>
        /// 5小时过期
        /// </summary>

        public const int FIVE_HOURS = 18000;

        /// <summary>
        /// 3小时过期
        /// </summary>

        public const int THREE_HOURS = 10800;

        /// <summary>
        /// 2小时过期
        /// </summary>

        public const int TWO_HOURS = 7200;

        /// <summary>
        /// 1小时过期
        /// </summary>

        public const int ONE_HOURS = 3600;

        /// <summary>
        /// 半小时过期
        /// </summary>

        public const int HALF_HOURS = 1800;

        /// <summary>
        /// 5分钟过期
        /// </summary>
        public const int FIVE_MINUTES = 300;

        /// <summary>
        /// 1分钟过期
        /// </summary>
        public const int ONE_MINUTE = 60;

        /// <summary>
        /// 30秒过期
        /// </summary>
        public const int THIRTY_SECOND = 30;

        /// <summary>
        /// 20秒过期
        /// </summary>
        public const int TWENTY_SECOND = 10;

        /// <summary>
        /// 10秒过期
        /// </summary>
        public const int TEN_SECOND = 10;

        /// <summary>
        /// 永不过期
        /// </summary>

        public const int NEVER = -1;
    }
}
