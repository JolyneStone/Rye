using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Rye
{
    public static class EnumExtensions
    {
        private static readonly Dictionary<Enum, string> _descriptionsDict = new();
        private static readonly Dictionary<Enum, KeyValuePair<string, string>> _langDict = new();
        private static readonly object _descriptionLocker = new();
        private static readonly object _langLocker = new();

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

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> GetLang(this Enum @enum)
        {
            if (_langDict.TryGetValue(@enum, out var pair))
            {
                return pair;
            }
            try
            {
                string name = @enum.ToString();
                var attribute = @enum.GetType().GetField(name).GetCustomAttribute<LangAttribute>();
                if (attribute != null)
                {
                    if (attribute.LangKey == null)
                    {
                        pair = new KeyValuePair<string, string>(name, attribute.LangValue);
                    }
                    else
                    {
                        pair = new KeyValuePair<string, string>(attribute.LangKey, attribute.LangValue);
                    }
                }
                lock (_langLocker)
                {
                    _langDict[@enum] = pair;
                }
                return pair;
            }
            catch (Exception)
            {
                return new KeyValuePair<string, string>("", "");
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
