using Rye.CodeGenerator.Razor;
using System.Linq;
using System.Text;

namespace Rye.CodeGenerator.MySql
{
    public abstract class MySqlRazorPageView : RazorPageViewBase<ModelEntity>
    {
        /// <summary>
        /// 获取主键参数列表
        /// </summary>
        /// <returns></returns>
        protected string GetPrimaryKeyParams()
        {
            if (Model == null)
                return "";
            StringBuilder strParameter = new StringBuilder();
            foreach (var c in Model.Properties)
            {
                if (c.IsKey)
                {
                    strParameter.AppendFormat("{0} {1},", c.Type, GetParameterName(c.Name));
                }
            }
            if (strParameter.ToString().Length > 1)
            {
                return strParameter.ToString().Remove(strParameter.ToString().Length - 1);
            }

            return "";
        }

        public string GetPrimaryKeyParamsValue()
        {
            StringBuilder strParameter = new StringBuilder();
            foreach (var c in Model.Properties)
            {
                if (c.IsKey)
                {
                    strParameter.AppendFormat("{0},", GetParameterName(c.Name));
                }
            }
            if (strParameter.ToString().Length > 1)
            {
                return strParameter.ToString().Remove(strParameter.ToString().Length - 1);
            }

            return "";
        }

        protected ModelProperty[] GetPKColumns()
        {
            return Model.Properties != null ? Model.Properties.Where(d => d.IsKey).ToArray() : new ModelProperty[0];
        }

        protected ModelProperty[] GetNoPKAndIdentityColumns()
        {
            return Model.Properties != null ? Model.Properties.Where(d => !(d.IsKey || d.IsIdentity)).ToArray() : new ModelProperty[0];
        }

        /// <summary>
        /// 表是否有标识主键
        /// </summary>
        /// <returns></returns>
        protected bool HasIdentityPK(out ModelProperty property)
        {
            if (Model.Properties == null)
            {
                property = null;
                return false;
            }
            property = Model.Properties.FirstOrDefault(c => c.IsKey && c.IsIdentity);
            return property != null;
        }

        /// <summary>
        /// 表是否有标识列
        /// </summary>
        /// <returns></returns>
        protected bool HasIdentity(out ModelProperty property)
        {
            if (Model.Properties == null)
            {
                property = null;
                return false;
            }
            property = Model.Properties.Where(c => c.IsIdentity).OrderByDescending(d => d.IsKey).FirstOrDefault();
            return property != null;
        }

        protected string GetInsertSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0} (", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("{0},", c.SqlColumn);
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(") VALUES (");
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("@{0},", c.Name);
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(");");

            return strSql.ToString();
        }

        protected string GetInsertModelSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0} (", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("{0},", c.SqlColumn);
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(") VALUES (");
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("@{0},", c.SqlColumn);
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(");");

            if (HasIdentityPK(out var _))
            {
                strSql.Append("SELECT ");
                string IdentityWhere = "";
                foreach (var c in Model.Properties)
                {
                    strSql.AppendFormat("{0},", c.SqlColumn);
                    if (c.IsKey && c.IsIdentity)
                    {
                        IdentityWhere = c.SqlColumn;
                    }
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);
                strSql.AppendFormat(" FROM {0} WHERE {1}=LAST_INSERT_ID()", Model.SqlTable, IdentityWhere);
            }
            else
            {
                var columns = GetPKColumns();
                if (columns.Length > 0)
                {
                    strSql.Append(";SELECT ");
                    foreach (var c in Model.Properties)
                    {
                        strSql.AppendFormat("{0},", c.SqlColumn);
                    }
                    strSql = strSql.Remove(strSql.Length - 1, 1);
                    strSql.AppendFormat(" FROM {0} WHERE 1=1", Model.SqlTable);
                    foreach (var c in columns)
                    {
                        strSql.AppendFormat(" AND {0}=@{0}", c.SqlColumn);
                    }
                }
            }
            return strSql.ToString();
        }

        protected string GetBatchInsertSql()
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0} (", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("{0},", c.SqlColumn);
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(") VALUES ");
            return strSql.ToString();
        }

        protected string GetBatchInsertValuesSql()
        {
            var strSql = new StringBuilder("(");
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.Append("{item." + c.Name + "},");
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append("),");

            return strSql.ToString();
        }

        protected string GetExistsSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT 1 FROM {0} ", Model.SqlTable);
            var columns = GetPKColumns();
            if (columns.Length > 0)
            {
                strSql.Append(" WHERE 1=1 ");
                foreach (var c in columns)
                {
                    strSql.AppendFormat(" AND {0}=@{1}", c.SqlColumn, c.Name);
                }
            }
            else
            {
                strSql.Append(" WHERE AND 1<>1");
            }

            strSql.Append(" LIMIT 1");
            return strSql.ToString();
        }

        protected string GetCountSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT COUNT(1) FROM {0} ", Model.SqlTable);
            return strSql.ToString();
        }


        protected string GetDeleteSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM {0}", Model.SqlTable);
            var columns = GetPKColumns();
            if (columns.Length > 0)
            {
                strSql.Append(" WHERE 1=1");
                foreach (var c in columns)
                {
                    strSql.AppendFormat(" AND {0}=@{1}", c.SqlColumn, c.Name);
                }
            }
            else
            {
                strSql.Append("WHERE AND 1<>1");
            }

            return strSql.ToString();
        }

        protected string GetUpdateSql()
        {
            StringBuilder strSql = new StringBuilder();
            var nonColumns = GetNoPKAndIdentityColumns();
            if (nonColumns.Length > 0)
            {
                strSql.AppendFormat("UPDATE {0} SET ", Model.SqlTable);
                foreach (var c in nonColumns)
                {
                    strSql.AppendFormat(" {0}=@{1},", c.SqlColumn, c.Name);
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);


                var columns = GetPKColumns();
                if (columns.Length > 0)
                {
                    strSql.Append(" WHERE 1=1 ");
                    foreach (var k in columns)
                    {
                        strSql.AppendFormat(" AND {0}=@{1}", k.SqlColumn, k.Name);
                    }
                }
                else
                {
                    strSql.Append(" WHERE 1<>1");
                }
            }

            return strSql.ToString();
        }

        protected string GetSelectSql()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT ");
            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("{0} {1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM {0} WHERE 1=1", Model.SqlTable);
            foreach (var c in GetPKColumns())
            {
                strSql.AppendFormat(" AND {0}=@{1}", c.SqlColumn, c.Name);
            }

            return strSql.ToString();
        }

        protected string GetSelectWithNoParamSql()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT ");
            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("{0} {1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM {0} LIMIT 1", Model.SqlTable);
            return strSql.ToString();
        }

        protected string GetFirstOrDefaultWithNoParamSql()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT ");
            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("{0} {1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM {0} LIMIT 1", Model.SqlTable);
            return strSql.ToString();
        }

        protected string GetSelectColumnsSql()
        {
            StringBuilder strSql = new StringBuilder();

            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("{0} {1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);

            return strSql.ToString();
        }

        protected string GetSelectAllSql()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT ");
            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("{0} {1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM {0} ", Model.SqlTable);
            var columns = GetPKColumns();
            if (columns.Length > 0)
            {
                strSql.Append(" ORDER BY ");
                foreach (var c in columns)
                {
                    strSql.AppendFormat("{0} DESC,", c.SqlColumn);
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);
            }
            return strSql.ToString();
        }

        protected string GetSelectPageSql()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT ");
            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("{0} {1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM {0} ", Model.SqlTable);
            strSql.Append(" WHERE {0} ORDER BY {1} LIMIT {3},{2}");
            return strSql.ToString();
        }

        public string GetInsertUpdateSql()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat("UPDATE {0} SET ", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsKey)
                    strSql.AppendFormat(" {0}=@{1},", c.SqlColumn, c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);

            strSql.Append(" WHERE 1=1 ");

            foreach (var c in Model.Properties)
            {
                if (c.IsKey)
                    strSql.AppendFormat(" AND {0}=@{1}", c.SqlColumn, c.Name);
            }

            strSql.Append(";");

            strSql.AppendFormat("INSERT INTO {0} (", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                    strSql.AppendFormat("{0},", c.SqlColumn);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(") SELECT ");
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                    strSql.AppendFormat("@{0},", c.Name);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            //strSql.Append(")");    

            strSql.AppendFormat(" WHERE NOT EXISTS (SELECT 1 FROM {0} where 1=1 ", Model.SqlTable);

            foreach (var c in Model.Properties)
            {
                if (c.IsKey || c.IsIdentity)
                    strSql.AppendFormat(" AND {0}=@{1}", c.SqlColumn, c.Name);
            }
            strSql.Append(")");
            return strSql.ToString();
        }

        public string GetParameterName(string name)
        {
            return name.ToLowerCamelCase();
        }
    }
}
