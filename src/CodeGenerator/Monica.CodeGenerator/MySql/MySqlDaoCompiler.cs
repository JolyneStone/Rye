namespace Monica.CodeGenerator.MySql
{
    public class MySqlDaoCompiler : MySqlCompiler
    {
        protected override string GetFileName(ModelEntity entity)
        {
            return $"Dao{entity.Name}.cs";
        }

        protected override string GetTemplate()
        {
            return "MySqlDao.tp";
        }
    }
}
