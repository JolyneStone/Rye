using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monica.CodeGenerator
{
    public class ModelEntity
    {
        public ModelEntity()
        {
        }

        public ModelEntity(TableFeature table, IEnumerable<ColumnFeature> columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            SqlTable = table.Name;
            Name = table.Name.ToUpperCamelCase();
            Schema = table.Schema;
            Description = table.Description;

            var properties = new List<ModelProperty>(columns.Count());
            foreach (var column in columns)
            {
                properties.Add(new ModelProperty
                {
                    Name = column.Name.ToUpperCamelCase(),
                    SqlColumn = column.Name,
                    Description = column.Description,
                    IsKey = column.IsKey,
                    IsIdentity = column.IsIdentity,
                    IsNullable = column.IsNullable,
                    SqlDefaultValue = column.DefaultValue,
                    DefaultValue = Util.GetDefaultValueString(Util.GetCSharpType(column.SqlType, column.IsNullable), column.DefaultValue),
                    SqlType = column.SqlType,
                    Type = Util.GetCSharpType(column.SqlType, column.IsNullable)
                });
            }

            Properties = properties;
        }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// Schema
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SQL表名
        /// </summary>
        public string SqlTable { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 属性列表
        /// </summary>
        public List<ModelProperty> Properties { get; set; }
    }

    public class ModelProperty
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SQL字段名
        /// </summary>
        public string SqlColumn { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// SQL类型
        /// </summary>
        public string SqlType { get; set; }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// SQL默认值
        /// </summary>
        public string SqlDefaultValue { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
