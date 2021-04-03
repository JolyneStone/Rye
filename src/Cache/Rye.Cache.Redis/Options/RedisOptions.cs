namespace Rye.Cache.Redis.Options
{
    public class RedisOptions
    {
        /// <summary>
        /// Redis 连接名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Redis 地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Redis 端口，默认为6379
        /// </summary>
        public int Port { get; set; } = 6379;
        /// <summary>
        /// Redis 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Redis 默认数据库编号，默认为0
        /// </summary>
        public int DefaultDatabase { get; set; } = 0;
        /// <summary>
        /// Redis 默认连接池大小，默认为50
        /// </summary>
        public int PoolSize { get; set; } = 50;
        /// <summary>
        /// 写入缓冲区大小，默认为10240
        /// </summary>
        public int WriteBuffer { get; set; } = 10240;
        /// <summary>
        /// Key前缀
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// 同步超时时间
        /// </summary>
        public int SyncTimeout { get; set; }
        /// <summary>
        /// 空闲连接超时时间
        /// </summary>
        public int IdleTimeout { get; set; }
        /// <summary>
        /// 是否使用SSL连接
        /// </summary>
        public bool Ssl { get; set; }
        /// <summary>
        /// 是否是只读的
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}
