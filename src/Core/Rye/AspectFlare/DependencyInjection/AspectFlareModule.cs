//using Microsoft.Extensions.DependencyInjection;
//using Rye.Enums;
//using Rye.Module;

//namespace Rye.AspectFlare.DependencyInjection
//{
//    /// <summary>
//    /// 动态代理模块
//    /// </summary>
//    public class AspectFlareModule: StartupModule
//    {
//        public override ModuleLevel Level => ModuleLevel.Core;
//        public override uint Order => 0;

//        public override void ConfigueServices(IServiceCollection services)
//        {
//            services.UseDynamicProxyService(true);
//        }
//    }
//}
