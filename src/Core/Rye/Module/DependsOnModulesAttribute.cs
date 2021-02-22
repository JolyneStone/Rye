using System;

namespace Rye.Module
{
    /// <summary>
    /// 模块依赖
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DependsOnModulesAttribute : Attribute
    {
        public DependsOnModulesAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }

        /// <summary>
        /// 获取 当前模块的依赖模块类型集合
        /// </summary>
        public Type[] DependedModuleTypes { get; }
    }
}
