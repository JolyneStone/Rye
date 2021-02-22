using Rye.Entities.Abstractions;
using System.Collections.Generic;

namespace Rye.Entities.Abstractions
{
    public interface ILangDictionaryService
    {
        IEnumerable<IEntityLangDictionaryBase> GetEnableList();
    }
}
