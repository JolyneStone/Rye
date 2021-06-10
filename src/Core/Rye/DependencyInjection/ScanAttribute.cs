using System;

namespace Rye.DependencyInjection
{
    /// <summary>
    /// 标识需要扫描的类型
    /// </summary>
    /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Enum,
        AllowMultiple = true, Inherited = true)]
    public class ScanAttribute : Attribute
    {
    }
}
