using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.CodeGenerator.SqlServer
{
    public class SqlServerModelConfig : ModelConfig
    {
        public SqlServerModelConfig()
        {
            Schema = "dbo";
        }
    }

    public class SqlServerModelEntity: ModelEntity
    {
        public SqlServerModelEntity()
        {
            Schema = "dbo";
        }
    }
}
