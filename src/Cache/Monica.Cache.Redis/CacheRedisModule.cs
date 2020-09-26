using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.Cache.Redis
{
    /// <summary>
    /// 添加Redis缓存模块
    /// </summary>
    /// <returns></returns>
    [DependsOnModules(typeof(CacheModule))]
    public class CacheRedisModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;
        public override uint Order => 10;

        private Action<RedisCacheOptions> _action;

        public CacheRedisModule(Action<RedisCacheOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddMonicaCacheRedis(_action);
        }
    }
}
