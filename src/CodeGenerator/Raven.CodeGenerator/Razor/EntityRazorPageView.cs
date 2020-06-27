namespace Raven.CodeGenerator.Razor
{
    public abstract class EntityRazorPageView : RazorPageViewBase<ModelEntity>
    {
        /// <summary>
        /// 获取主键参数列表
        /// </summary>
        /// <returns></returns>
        protected string GetPrimaryKeyParams()
        {
            if (Model == null)
                return "";
            var s = string.Empty;
            foreach(var column in Model.Properties)
            {
                if (column.IsKey)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = column.Type + column.Name.LowerFirstChar();
                    }
                    else
                    {
                        s += ", " + column.Type + column.Name.LowerFirstChar();
                    }
                }
            }

            return s;
        }
    }
}
