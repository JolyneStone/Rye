using Rye.Cache.Redis.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Cache.Redis.Internal
{
    public class RedisConnectionBuilder
    {
        public string BuildConnectionString(RedisOptions options)
        {
            var sb = new StringBuilder(options.Host ?? "127.0.0.1");
            sb.Append(":" + options.Port);
            if (!string.IsNullOrEmpty(options.Password))
            {
                sb.Append(",password=" + options.Password);
            }

            sb.Append(",defaultdatabase=" + options.DefaultDatabase);
            sb.Append(",poolsize=" + options.PoolSize);
            sb.Append(",writebuffer=" + options.WriteBuffer);
            sb.Append(",prefix=" + options.Prefix);
            sb.Append(",ssl=" + (options.Ssl ? "true" : "false"));
            sb.Append(",connecttimeout=" + options.ConnectTimeout);
            sb.Append(",synctimeout=" + options.SyncTimeout);
            sb.Append(",idletimeout=" + options.IdleTimeout);

            return sb.ToString();
        }
       
    }
}
