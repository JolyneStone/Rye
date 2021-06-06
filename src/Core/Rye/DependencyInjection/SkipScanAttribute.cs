using System;

namespace Rye.DependencyInjection
{
    /// <summary>
    /// 跳过扫描
    /// </summary>
    /// <remarks></remarks>
    [SkipScan, AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Enum)]
    public class SkipScanAttribute : Attribute
    {
    }
}
