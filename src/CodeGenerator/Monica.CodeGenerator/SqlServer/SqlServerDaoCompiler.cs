namespace Monica.CodeGenerator.SqlServer
{
    public class SqlServerDaoCompiler : SqlServerCompiler
    {
        protected override string GetFileName(ModelEntity entity)
        {
            return $"Dao{entity.Name}.cs";
        }

        protected override string GetTemplate()
        {
            return "SqlServerDao.tp";
        }
    }
}
