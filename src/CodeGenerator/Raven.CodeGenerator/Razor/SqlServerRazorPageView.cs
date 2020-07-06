using System.Linq;
using System.Text;

namespace Raven.CodeGenerator.Razor
{
    public abstract class SqlServerRazorPageView : RazorPageViewBase<ModelEntity>
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
            foreach (var column in Model.Properties)
            {
                if (column.IsKey)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = column.Type + " " + column.Name.LowerFirstChar();
                    }
                    else
                    {
                        s += ", " + column.Type + " " + column.Name.LowerFirstChar();
                    }
                }
            }

            return s;
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
        /// 表是否有标识列
        /// </summary>
        /// <returns></returns>
        protected bool HasIdentityPK()
        {
            return Model.Properties != null && Model.Properties.Any(c => c.IsKey && c.IsIdentity);
        }

        protected string GetInsertSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO [{0}] (", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("[{0}],", c.SqlColumn);
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

            return strSql.ToString();
        }

        protected string GetInsertModelSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO [{0}] (", Model.SqlTable);
            foreach (var c in Model.Properties)
            {
                if (!c.IsIdentity)
                {
                    strSql.AppendFormat("[{0}],", c.SqlColumn);
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

            if (HasIdentityPK())
            {
                strSql.Append("SELECT ");
                string IdentityWhere = "";
                foreach (var c in Model.Properties)
                {
                    strSql.AppendFormat("[{0}],", c.SqlColumn);
                    if (c.IsKey && c.IsIdentity)
                    {
                        IdentityWhere = c.SqlColumn;
                    }
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);
                strSql.AppendFormat(" FROM [{0}] WHERE [{1}]=IDENT_CURRENT('{2}')", Model.SqlTable, IdentityWhere, Model.SqlTable);
            }
            else
            {
                var columns = GetPKColumns();
                if (columns.Length > 0)
                {
                    strSql.Append(";SELECT ");
                    foreach (var c in Model.Properties)
                    {
                        strSql.AppendFormat("[{0}],", c.SqlColumn);
                    }
                    strSql = strSql.Remove(strSql.Length - 1, 1);
                    strSql.AppendFormat(" FROM [{0}] WHERE 1=1", Model.SqlTable);
                    foreach (var c in columns)
                    {
                        strSql.AppendFormat(" AND [{0}]=@{0}", c.SqlColumn);
                    }
                }
            }
            return strSql.ToString();
        }

        protected string GetExistsSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT COUNT(1) FROM [{0}] ", Model.SqlTable);
            var columns = GetPKColumns();
            if (columns.Length > 0)
            {
                strSql.Append(" WHERE 1=1 ");
                foreach (var c in columns)
                {
                    strSql.AppendFormat(" AND [{0}]=@{0}", c.SqlColumn);
                }
            }
            else
            {
                strSql.Append(" WHERE AND 1<>1");
            }
            return strSql.ToString();
        }

        protected string GetDeleteSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM [{0}]", Model.SqlTable);
            var columns = GetPKColumns();
            if (columns.Length > 0)
            {
                strSql.Append(" WHERE 1=1");
                foreach (var c in columns)
                {
                    strSql.AppendFormat(" AND [{0}]=@{0}", c.SqlColumn);
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
                strSql.AppendFormat("UPDATE [{0}] SET ", Model.SqlTable);
                foreach (var c in nonColumns)
                {
                    strSql.AppendFormat(" [{0}]=@{0},", c.SqlColumn);
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);


                var columns = GetPKColumns();
                if (columns.Length > 0)
                {
                    strSql.Append(" WHERE 1=1 ");
                    foreach (var k in columns)
                    {
                        strSql.AppendFormat(" AND [{0}]=@{0}", k.SqlColumn);
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
                strSql.AppendFormat("[{0}],", c.SqlColumn);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM [{0}] WHERE 1=1", Model.SqlTable);
            foreach(var c in GetPKColumns())
            {
                strSql.AppendFormat(" AND [{0}]=@{0}", c.SqlColumn);
            }

            return strSql.ToString();
        }

        protected string GetSelectColumnsSql()
        {
            StringBuilder strSql = new StringBuilder();

            foreach (var c in Model.Properties)
            {
                strSql.AppendFormat("[{0}],", c.SqlColumn);
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
                strSql.AppendFormat("[{0}],", c.SqlColumn);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM [{0}] ", Model.SqlTable);
            var columns = GetPKColumns();
            if (columns.Length > 0)
            {
                strSql.Append(" ORDER BY ");
                foreach(var c in columns)
                {
                    strSql.AppendFormat("[{0}] DESC,", c.SqlColumn);
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
                strSql.AppendFormat("[{0}],", c.SqlColumn);
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendFormat(" FROM [{0}]", Model.SqlTable);
            strSql.Append(" WHERE {0} ORDER BY {1} offset {2} rows fetch next {3} rows only");
            return strSql.ToString();
        }
    }
}
