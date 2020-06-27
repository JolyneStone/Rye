using System;
using System.Collections.Generic;
using System.Text;

namespace Raven.CodeGenerator.SqlServer
{
    public class SqlServerInterfaceCompiler : SqlServerCompiler
    {
        protected override string GetFileName(ModelEntity entity)
        {
            return "I" + entity.Name + ".cs";
        }

        protected override string GetTemplate()
        {
            return "SqlDataAccess_Interface.tp";
        }
    }
}
