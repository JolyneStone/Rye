namespace Rye.CodeGenerator.SqlServer
{
    public class SqlServerInterfaceCompiler : SqlServerCompiler
    {
        protected override string GetFileName(ModelEntity entity)
        {
            return "I" + entity.Name + ".cs";
        }

        protected override string GetTemplate()
        {
            return "Interface.tp";
        }
    }
}
