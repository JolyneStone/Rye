
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Rye
{
    public static class EnumExtensions
    {
        private static readonly Dictionary<Enum, string> _descriptionsDict = new Dictionary<Enum, string>();
        private static readonly Dictionary<Enum, string> _langKeyDict = new Dictionary<Enum, string>();
        private static readonly object _descriptionLocker = new object();
        private static readonly object _langKeyLocker = new object();
        public static string GetDescription(this Enum @enum)
        {
            if (_descriptionsDict.ContainsKey(@enum))
            {
                return _descriptionsDict[@enum];
            }
            try
            {
                string name = @enum.ToString();
                var attribute = @enum.GetType().GetField(name).GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    name = attribute.Description;
                }
                lock (_descriptionLocker)
                {
                    _descriptionsDict[@enum] = name;
                }
                return name;
            }
            catch (Exception)
            {
                return "枚举错误";
            }
        }

        public static string GetLangKey(this Enum @enum)
        {

            if (_langKeyDict.ContainsKey(@enum))
            {
                return _langKeyDict[@enum];
            }
            try
            {
                string langKey = @enum.ToString();
                var attribute = @enum.GetType().GetField(langKey).GetCustomAttribute<LangKeyAttribute>();
                if (attribute != null)
                {
                    langKey = attribute.LangKey;
                }

                lock (_langKeyLocker)
                {
                    _langKeyDict[@enum] = langKey;
                }
                return langKey;
            }
            catch (Exception)
            {
                return "GetLangKey枚举错误";
            }
        }
        public static int Value(this Enum item)
        {
            return item.GetHashCode();
        }
        public static byte ToByte(this Enum item)
        {
            return Convert.ToByte(item);
        }
        public static short ToShort(this Enum item)
        {
            return Convert.ToInt16(item);
        }
    }
}
