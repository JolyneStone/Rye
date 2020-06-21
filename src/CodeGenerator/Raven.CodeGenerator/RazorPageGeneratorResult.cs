using System;
using System.Collections.Generic;
using System.Text;

namespace Raven.CodeGenerator
{
    public class RazorPageGeneratorResult
    {
        public string ClassName { get; set; }
        public string FilePath { get; set; }
        public string GeneratedCode { get; set; }
    }
}
