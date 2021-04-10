
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.Cache.Redis.Options;
using Rye.Enums;
using Rye.Module;

using System;

namespace Rye.Cache.Redis
{
    /// <summary>
    /// 添加Redis缓存模块
    /// </summary>
    /// <returns></returns>
    [DependsOnModules(typeof(CacheModule))]
    public class RedisCacheModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;
        public override uint Order => 1;

        private readonly Action<RedisOptions> _action;

        public RedisCacheModule(Action<RedisOptions> action)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.RemoveAll<IDistributedCache>();
            services.AddRedisCache(_action);
        }
    }
}
