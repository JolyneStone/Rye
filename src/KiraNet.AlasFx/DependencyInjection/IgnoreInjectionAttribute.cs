using System;

namespace KiraNet.AlasFx.DependencyInjection
{
    /// <summary>
    /// 标注了此特性的类，将忽略依赖注入自动映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreInjectionAttribute : Attribute
    {
    }
}
