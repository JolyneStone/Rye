using Rye.Entities.Abstractions;
using System.Collections.Generic;

namespace Rye.Entities.Abstractions
{
    public interface ILangDictionaryService
    {
        IEnumerable<(string lang, string dicKey, string dicValue)> GetEnableList();
    }
}
