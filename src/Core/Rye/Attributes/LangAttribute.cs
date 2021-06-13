using System;

namespace Rye
{
    /// <summary>
    /// 配置多语言Key和Value的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LangAttribute : Attribute
    {
        private readonly string _langKey;
        private readonly string _langValue;
        public LangAttribute(string key)
        {
            _langKey = key;
            _langValue = string.Empty;
        }

        public LangAttribute(string key, string value)
        {
            _langKey = key;
            _langValue = value;
        }

        public string LangKey => _langKey;

        public string LangValue => _langValue;
    }
}
