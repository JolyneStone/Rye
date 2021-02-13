using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.DataAccess.EFCore.Models
{
    public partial class LangDictionary
    {
        public string DicKey { get; set; }
        public string Lang { get; set; }
        public string DicValue { get; set; }
    }
}
