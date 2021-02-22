using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Rye
{
    public static class EnumExtensions
    {
        private static readonly Dictionary<Enum, string> enumDescriptions = new Dictionary<Enum, string>();
        private static readonly Dictionary<Enum, string> enumLangKey = new Dictionary<Enum, string>();
        private static readonly object _lock = new object();
        public static string GetDescription(this Enum @enum)
        {
            try
            {
                if (enumDescriptions.ContainsKey(@enum))
                {
                    return enumDescriptions[@enum];
                }
                lock (_lock)
                {
                    string name = @enum.ToString();
                    var attribute = @enum.GetType().GetField(name).GetCustomAttribute<DescriptionAttribute>();
                    if (attribute != null)
                    {
                        name = attribute.Description;
                    }
                    enumDescriptions[@enum] = name;
                    return name;
                }
            }
            catch (Exception)
            {
                return "枚举错误";
            }
        }

        public static string GetLangKey(this Enum @enum)
        {
            try
            {
                if (enumLangKey.ContainsKey(@enum))
                {
                    return enumLangKey[@enum];
                }

                lock (_lock)
                {
                    string langKey = @enum.ToString();
                    var attribute = @enum.GetType().GetField(langKey).GetCustomAttribute<LangKeyAttribute>();
                    if (attribute != null)
                    {
                        langKey = attribute.LangKey;
                    }

                    enumLangKey[@enum] = langKey;
                    return langKey;
                }
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
