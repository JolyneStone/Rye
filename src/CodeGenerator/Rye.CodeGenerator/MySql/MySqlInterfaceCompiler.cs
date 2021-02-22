namespace Rye.CodeGenerator.MySql
{
    public class MySqlInterfaceCompiler : MySqlCompiler
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
