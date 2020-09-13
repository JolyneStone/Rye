using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Monica.AspectFlare.DynamicProxy
{
    public class ProxyConfiguration : IProxyConfiguration
    {
        public AppDomain ProxyDomain { get; private set; }
        public ModuleBuilder ProxyModuleBuilder { get; private set; }
        public AssemblyName ProxyAssemblyName { get; private set; }
        public AssemblyBuilder ProxyAssemblyBuilder { get; private set; }

        public ProxyConfiguration()
        {
            ProxyDomain = AppDomain.CurrentDomain;
            ProxyAssemblyName = new AssemblyName("Monica.AspectFlare.DynamicProxy.Dynamic");
            ProxyAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(ProxyAssemblyName, AssemblyBuilderAccess.RunAndCollect);
            ProxyModuleBuilder = ProxyAssemblyBuilder.DefineDynamicModule("DynamicModule");
        }
    }
}
