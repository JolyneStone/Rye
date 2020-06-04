using System;
using System.Collections.Generic;

namespace KiraNet.CodeGenerator
{
    public class CodeConfig
    {
        public string FilePath { get; set; }
        public string NameSpace { get; set; }
    }

    public class DbCodeConfig : CodeConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }

    public class ModelConfig : DbCodeConfig
    {
        public string Schema { get; set; }
        public string Table { get; set; }
    }



    public class ModelEntity
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public List<ModelPropertyConfig> Properties { get; set; }
    }

    public class ModelPropertyConfig
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}
