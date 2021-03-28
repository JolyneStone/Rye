using Rye.Cache;
using Rye.Cache.Internal;
using Rye.Entities.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Language
{
    public class LangService : ILangService
    {
        private readonly ILangDictionaryService _langDictionaryService;

        public LangService(ILangDictionaryService langDictionaryService)
        {
            _langDictionaryService = langDictionaryService;
        }

        private Dictionary<string, Dictionary<string, string>> GetEnableDictionary()
        {
            var entry = MemoryCacheEntryCollection.GetLangDictionaryListEntry();
            var dictionary = MemoryCacheManager.Get<Dictionary<string, Dictionary<string, string>>>(entry.CacheKey);
            if (dictionary == null)
            {
                IEnumerable<(string lang, string dicKey, string dicValue)> _list = _langDictionaryService.GetEnableList();
                if (_list != null && _list.Any())
                {
                    dictionary = new Dictionary<string, Dictionary<string, string>>();
                    foreach (var item in _list.GroupBy(d => d.dicKey))
                    {
                        dictionary.Add(item.Key, item.ToDictionary(d => d.lang, d => d.dicValue));
                    }
                    MemoryCacheManager.Set(entry.CacheKey, dictionary, entry.Expire);
                    return dictionary;
                }
            }
            return dictionary;
        }

        public string Get(string lang, string dicKey, string defaultResult = "")
        {
            var dictionary = GetEnableDictionary();
            if (dictionary == null || !dictionary.ContainsKey(dicKey))
                return defaultResult;

            if (string.IsNullOrEmpty(lang))
                lang = LangCode.ZHCN;

            var langDictionary = dictionary[dicKey];
            if (langDictionary == null)
                return defaultResult;

            return langDictionary[lang] ?? defaultResult;
        }

        public string Get(string langCode, Enum e)
        {
            return Get(langCode, e.GetLangKey(), e.GetDescription());
        }
    }
}
