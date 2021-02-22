using Rye.Entities.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.EFCore.Models
{
    public partial class LangDictionary : IEntityLangDictionaryBase
    {
        public string Key => $"{DicKey}_{Lang}";
    }
}
