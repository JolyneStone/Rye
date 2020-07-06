using System;
using System.Collections.Generic;
using System.Linq;

namespace Raven.CodeGenerator
{
    public class CodeConfig
    {
        public string FilePath { get; set; }
        public string NameSpace { get; set; }
    }

    public class DbCodeConfig : CodeConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }

    public class ModelConfig : DbCodeConfig
    {
        public string Schema { get; set; }
        public string Table { get; set; }
    }
}
