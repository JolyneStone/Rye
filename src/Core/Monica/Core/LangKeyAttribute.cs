using System;

namespace Monica
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class LangKeyAttribute:Attribute
    {
        private string _langKey = "";
        public LangKeyAttribute(string langKey)
        {
            this._langKey = langKey;
        }

        public string LangKey
        {
            get
            {
                if (string.IsNullOrEmpty(_langKey))
                {
                    _langKey = "UnknownError";
                }
                return _langKey;
            }
        }
    }
}
