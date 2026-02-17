using Rye.SqlSugarCore.Domains;
using Rye.SqlSugarCore.Util;
using SqlSugar;
using System.Reflection;
using System.Xml.Linq;

namespace Rye.SqlSugarCore.Init
{
    public class TableFirst : RyeDbContext
    {
        /// <summary>
        /// 初始化表结构
        /// </summary>
        /// <param name="entity"></param>
        public void InitTables(Type entity)
        {
            SugarTable customAttribute = entity.GetCustomAttribute<SugarTable>(inherit: false);
            SysTableInfoTable stit = new SysTableInfoTable
            {
                TenantId = 1,
                TableName = customAttribute.TableName,
                Description = (GetTableAnnotation(entity) ?? string.Empty),
                ColumnCount = 0
            };
            if (customAttribute.TableName.IsNullOrEmpty())
            {
                stit.TableName = entity.Name;
            }

            PropertyInfo[] properties = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            List<SysTableColumnTable> list = new List<SysTableColumnTable>();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                SugarColumn customAttribute2 = propertyInfo.GetCustomAttribute<SugarColumn>(inherit: false);
                if (customAttribute2 != null && !customAttribute2.IsIgnore)
                {
                    TableDict.ColumnTypeDict.TryGetValue(propertyInfo.PropertyType, out var value);
                    SysTableColumnTable sysTableColumnTable = new SysTableColumnTable
                    {
                        TenantId = 1,
                        ColumnName = customAttribute2.ColumnName,
                        ColumnType = (value ?? string.Empty),
                        Length = customAttribute2.Length,
                        DecimalDigits = customAttribute2.DecimalDigits,
                        IsPrimaryKey = customAttribute2.IsPrimaryKey,
                        IsNullable = customAttribute2.IsNullable,
                        Description = (GetPropertyAnnotation(entity, propertyInfo.Name) ?? string.Empty)
                    };
                    if (customAttribute2.ColumnName.IsNullOrEmpty())
                    {
                        sysTableColumnTable.ColumnName = propertyInfo.Name;
                    }

                    list.Add(sysTableColumnTable);
                }
            }

            SysTableInfoTable stol = (from w in Db.Queryable<SysTableInfoTable>()
                                      where w.TableName == stit.TableName
                                      select w).First();
            List<SysTableColumnTable> list2 = null;
            if (stol != null)
            {
                list2 = (from w in Db.Queryable<SysTableColumnTable>()
                         where w.TableId == stol.Id
                         select w).ToList();
            }

            stit.ColumnCount = list.Count;
            if (stol == null)
            {
                Db.Insertable(stit).ExecuteCommandIdentityIntoEntity();
            }
            else
            {
                stit.Id = stol.Id;
                Db.Updateable(stit).UpdateColumns((SysTableInfoTable u) => new { u.TenantId, u.TableName, u.Description, u.ColumnCount }).ExecuteCommand();
            }

            if (list2 == null)
            {
                if (list != null && list.Any())
                {
                    list = list.Select(delegate (SysTableColumnTable s)
                    {
                        s.TableId = stit.Id;
                        return s;
                    }).ToList();
                    Db.Insertable(list).ExecuteCommandIdentityIntoEntity();
                }

                return;
            }

            foreach (SysTableColumnTable sfit in list)
            {
                sfit.TableId = stit.Id;
                SysTableColumnTable sysTableColumnTable2 = list2.Where((SysTableColumnTable w) => w.ColumnName == sfit.ColumnName).FirstOrDefault();
                if (sysTableColumnTable2 != null)
                {
                    sfit.Id = sysTableColumnTable2.Id;
                }
            }

            Db.Storageable(list).ExecuteCommand();
        }

        /// <summary>
        /// 校验是否为类
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool IsClass(Type thisValue)
        {
            if (thisValue != typeof(string) && thisValue.IsEntity())
            {
                return thisValue != typeof(byte[]);
            }

            return false;
        }

        /// <summary>
        /// 获取类注释
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public string GetTableAnnotation(Type entityType)
        {
            if (!IsClass(entityType))
            {
                return null;
            }

            string xElementNodeValue = GetXElementNodeValue(entityType, "T:" + entityType.FullName);
            if (string.IsNullOrEmpty(xElementNodeValue))
            {
                return null;
            }

            return xElementNodeValue;
        }

        /// <summary>
        /// 获取属性注释
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="dbColumnName"></param>
        /// <returns></returns>
        public string GetPropertyAnnotation(Type entityType, string dbColumnName)
        {
            if (entityType == typeof(object))
            {
                return null;
            }

            string xElementNodeValue = GetXElementNodeValue(entityType, "P:" + entityType.FullName + "." + dbColumnName);
            if (string.IsNullOrEmpty(xElementNodeValue))
            {
                return GetPropertyAnnotation(entityType.BaseType, dbColumnName);
            }

            return xElementNodeValue;
        }

        /// <summary>
        /// 读取XML文档中的注释
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="nodeAttributeName"></param>
        /// <returns></returns>
        public string GetXElementNodeValue(Type entityType, string nodeAttributeName)
        {
            try
            {
                if (entityType.Assembly.IsDynamic && entityType.Assembly.FullName.StartsWith("Dynamic"))
                {
                    return null;
                }

                string location = entityType.Assembly.Location;
                if (string.IsNullOrEmpty(location))
                {
                    return null;
                }

                FileInfo fileInfo = new FileInfo(location);
                string xmlPath = entityType.Assembly.Location.Replace(fileInfo.Extension, ".xml");
                if (!File.Exists(xmlPath))
                {
                    return string.Empty;
                }

                XElement orCreate = new ReflectionInoCacheService().GetOrCreate("EntityXml_" + xmlPath, () => XElement.Load(xmlPath));
                if (orCreate == null)
                {
                    return string.Empty;
                }

                XElement xElement = (from ele in orCreate.Element("members").Elements("member")
                                     where ele.Attribute("name").Value == nodeAttributeName
                                     select ele).FirstOrDefault();
                if (xElement == null)
                {
                    return string.Empty;
                }

                XElement xElement2 = xElement.Element("summary");
                if (xElement2 != null)
                {
                    return xElement2.Value.ToSqlFilter().Trim();
                }

                string text = (from s in xElement.Elements()
                               where s.Name.ToString().Equals("summary", StringComparison.Ordinal)
                               select s.Value).FirstOrDefault();
                if (text == null)
                {
                    return string.Empty;
                }

                return text.ToSqlFilter().Trim() ?? "";
            }
            catch
            {
                SqlSugar.Check.ExceptionEasy("ORM error reading entity class XML, check entity XML or disable reading XML: MoreSettings IsNoReadXmlDescription set to true (same place to set DbType)", "ORM读取实体类的XML出现错误,检查实体XML或者禁用读取XML: MoreSettings里面的IsNoReadXmlDescription设为true （设置DbType的同一个地方）");
                throw;
            }
        }
    }
}
