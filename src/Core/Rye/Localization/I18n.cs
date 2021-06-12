using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Rye
{
    /// <summary>
    /// 国际化语言类
    /// </summary>
    public static class I18n
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _cultureDictionary =
         new Dictionary<string, Dictionary<string, string>>();
        private static readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 语言类型
        /// </summary>
        internal static Type LangType { get; set; }

        /// <summary>
        /// String 多语言
        /// </summary>
        public static IStringLocalizer @Text => LangType == null ? null : App.GetService<IStringLocalizerFactory>()?.Create(LangType);


        internal static Action<string> OnSetCultured { get; set; }

        internal static Func<CultureInfo> OnGetSelectCulturing { get; set; }

        /// <summary>
        /// 设置多语言区域
        /// </summary>
        /// <param name="culture"></param>
        public static void SetCulture(string culture)
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            OnSetCultured?.Invoke(culture);
        }

        /// <summary>
        /// 获取当前选择的语言
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetSelectCulture()
        {
            CultureInfo currentCultureInfo = null;
            if (OnGetSelectCulturing != null)
            {
                currentCultureInfo = OnGetSelectCulturing.Invoke();
            }

            if (currentCultureInfo == null)
                currentCultureInfo = CultureInfo.CurrentCulture;

            return currentCultureInfo;
        }

        /// <summary>
        /// 根据当前的CultureInfo获取对应语言的字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetText(string key, string defaultValue = "")
        {
            var culture = GetSelectCulture().Name;
            string val = null;
            Dictionary<string, string> dict = null;
            _rwLock.EnterReadLock();
            try
            {
                if (_cultureDictionary.TryGetValue(culture, out dict))
                {
                    if (dict.TryGetValue(key, out val))
                    {
                        return val;
                    }
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            if (val == null)
            {
                try
                {
                    val = Text[key];

                    if (!val.IsNullOrEmpty())
                    {
                        _rwLock.EnterWriteLock();
                        try
                        {
                            if (dict == null)
                            {
                                dict = new Dictionary<string, string>
                            {
                                { key, val }
                            };
                                _cultureDictionary[culture] = dict;
                            }
                            else
                            {
                                dict[key] = val;
                            }
                        }
                        finally
                        {
                            _rwLock.ExitWriteLock();
                        }
                    }
                }
                catch
                {
                }
            }

            return val.IsNullOrEmpty() ? defaultValue : val;
        }

        /// <summary>
        /// 根据当前的CultureInfo获取对应语言的字符串，并格式化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string GetText(string key, string defaultValue = "", params object[] arguments)
        {
            var val = GetText(key, defaultValue);
            if (!val.IsNullOrEmpty())
                val = string.Format(key, arguments);

            return val;
        }

        /// <summary>
        /// 根据当前的CultureInfo获取对应语言的字符串
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetText(Enum @enum)
        {
            var langInfo = @enum.GetLang();
            return GetText(langInfo.Key, langInfo.Value);
        }

        /// <summary>
        /// 根据当前的CultureInfo获取对应语言的字符串，并格式化
        /// </summary>
        /// <param name="enum"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string GetText(Enum @enum, params object[] arguments)
        {
            var langInfo = @enum.GetLang();
            return GetText(langInfo.Key, langInfo.Value, arguments);
        }

        /// <summary>
        /// 根据当前的CultureInfo获取对应语言的所有字符串
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetAllStrings()
        {
            var culture = GetSelectCulture().Name;

            Dictionary<string, string> dict = null;
            _rwLock.EnterReadLock();
            try
            {
                if (_cultureDictionary.TryGetValue(culture, out dict))
                {
                    return dict;
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            try
            {
                var list = Text.GetAllStrings();
                if (list != null)
                {
                    dict = new Dictionary<string, string>();
                    foreach (var item in list)
                    {
                        dict.Add(item.Name, item.Value);
                    }

                    _rwLock.EnterWriteLock();
                    try
                    {
                        _cultureDictionary[culture] = dict;
                    }
                    finally
                    {
                        _rwLock.ExitWriteLock();
                    }
                }
            }
            catch
            {
            }

            return dict;
        }
    }
}
