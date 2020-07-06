using System;
using System.Collections.Generic;
using System.Text;

namespace Raven.CodeGenerator
{
    public class TableFeature
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ColumnFeature
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public string SqlType { get; set; }
        public bool IsKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsNullable { get; set; }
    }
}
