namespace Monica.CodeGenerator.MySql
{
    public class MySqlModelCompiler : MySqlCompiler
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
