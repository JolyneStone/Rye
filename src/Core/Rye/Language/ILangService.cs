using System;

namespace Rye.Language
{
    /// <summary>
    /// 多语言服务
    /// </summary>
    public interface ILangService
    {
        string Get(string lang, string dicKey, string defaultResult = "");

        string Get(string langCode, Enum e);
    }
}
