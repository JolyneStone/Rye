namespace Raven.CodeGenerator.SqlServer
{
    public class SqlServerModelCompiler : SqlServerCompiler
    {
        protected override string GetFileName(ModelEntity entity)
        {
            return entity.Name + ".cs";
        }

        protected override string GetTemplate()
        {
            return "ModelObject.tp";
        }
    }
}