using Rye.Cache;
using Rye.Cache.Store;
using Rye.Entities.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Rye.Language
{
    public class LangService : ILangService
    {
        private readonly ILangDictionaryService _langDictionaryService;
        private readonly ICacheStore _store;

        public LangService(ILangDictionaryService langDictionaryService, ICacheStore store)
        {
            _langDictionaryService = langDictionaryService;
            _store = store;
        }

        private Dictionary<string, Dictionary<string, string>> GetEnableDictionary()
        {
            var entry = CacheEntryCollection.GetLangDictionaryListEntry();
            var dictionary = _store.Get<Dictionary<string, Dictionary<string, string>>>(entry);
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
                    _store.Set(entry, dictionary);
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
