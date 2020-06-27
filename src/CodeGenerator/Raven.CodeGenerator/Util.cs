using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace Raven.CodeGenerator
{
    internal class Util
    {
        /// <summary>
        /// 转化C#类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string GetCSharpType(string dbType, bool allowNull)
        {
            string csType = "";
            switch (dbType.ToLower())
            {
                case "varchar":
                case "varchar2":
                case "nvarchar":
                case "nvarchar2":
                case "char":
                case "nchar":
                case "text":
                case "longtext":
                case "string":
                    csType = "string";
                    break;

                case "time":
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    csType = allowNull ? "DateTime?" : "DateTime";
                    break;
                case "datetimeoffset":
                    csType = allowNull ? "DateTimeOffset?" : "DateTimeOffset";
                    break;

                case "tinyint":
                    csType = allowNull? "byte?": "byte";
                    break;

                case "sbyte":
                case "unsigned tinyint":
                    csType = allowNull? "sbyte?" : "sbyte";
                    break;

                case "unsigned smallint":
                    csType = allowNull ? "ushort?" : "ushort";
                    break;

                case "unsigned":
                case "int":
                case "number":
                case "integer":
                case "mediumint":
                    csType = allowNull ? "int?" : "int";
                    break;

                case "unsigned int":
                case "unsigned number":
                case "unsigned integer":
                case "unsigned mediumint":
                    csType = allowNull ? "uint?" : "uint";
                    break;

                case "bigint":
                    csType = "long";
                    break;

                case "unsigned bigint":
                    csType = allowNull ? "ulong?" : "ulong";
                    break;

                case "float":
                    csType = allowNull ? "double?" : "double";
                    break;

                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "double":
                    csType = allowNull ? "decimal?" : "decimal";
                    break;

                case "real":
                    csType = allowNull ? "single?" : "single";
                    break;

                case "bit":
                    csType = allowNull ? "bool?" : "bool";
                    break;

                case "binary":
                case "varbinary":
                case "image":
                case "raw":
                case "long":
                case "long raw":
                case "blob":
                case "bfile":
                case "timestamp":
                    csType = "byte[]";
                    break;

                case "uniqueidentifier":
                    csType = allowNull ? "Guid?" : "Guid";
                    break;

                case "xml":
                case "json":
                    csType = "string";
                    break;

                case "variant":
                    csType = "object";
                    break;

                default:
                    csType = "object";
                    break;
            }

            return csType;
        }

        /// <summary>
        /// 获取默认值字符串形式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultValueStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetDefaultValueString(string type, string defaultValueStr)
        {
            var defaultValue = string.Empty;
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(defaultValueStr))
            {
                return defaultValue;
            }

            if (defaultValueStr[0] == '(')
            {
                var i = 1;
                for (; i < defaultValueStr.Length; i++)
                    if (defaultValueStr[i] != '(')
                        break;
                if (defaultValueStr.Length < 2 * (i - 1))
                {
                    throw new InvalidOperationException("无法解析的默认值");
                }

                defaultValueStr = defaultValueStr.Substring(i, defaultValueStr.Length - i * 2);
            }

            var lowerType = type.ToLowerInvariant();
            switch (defaultValueStr.Trim().ToLowerInvariant())
            {
                case "":
                    if(lowerType == "string")
                    {
                        defaultValue = "string.Empty";
                    }
                    break;
                case "getdate()":
                case "now()":
                    if (lowerType == "datetime" || lowerType == "datetime?")
                    {
                        defaultValue = "DateTime.Now";
                    }
                    else if(lowerType == "datetimeoffset" || lowerType == "datetimeoffset?")
                    {
                        defaultValue = "DateTimeOffset.Now";
                    }
                    break;
                case "newid()":
                case "uuid()":
                    if (lowerType == "guid" || lowerType == "guid?")
                    {
                        defaultValue = "Guid.NewGuid()";
                    }
                    else if (lowerType == "string")
                    {
                        defaultValue = Guid.NewGuid().ToString();
                    }
                    break;
                default:
                    try
                    {
                        var value = ConvertFromString(Type.GetType(type), defaultValueStr);
                        defaultValue = value != null ? value.ToString() : string.Empty;

                    }
                    catch
                    {
                        defaultValue = string.Empty;
                    }
                    break;
            }

            return defaultValue;
        }

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>转换后的对象</returns>
        private static object ConvertObject(object obj, Type type)
        {
            if (type == null) return obj;
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString())) // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertObject(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }

        /// <summary>
        /// 将字符串格式化成指定的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object ConvertFromString(Type type, string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            if (type == null)
                return str;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                string[] strs = str.Split(new char[] { ';' });
                Array array = Array.CreateInstance(elementType, strs.Length);
                for (int i = 0, c = strs.Length; i < c; ++i)
                {
                    array.SetValue(ConvertSimpleType(strs[i], elementType), i);
                }
                return array;
            }
            return ConvertSimpleType(str, type);
        }

        private static object ConvertSimpleType(object value, Type destinationType)
        {
            object returnValue;
            if ((value == null) || destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                throw new InvalidOperationException("无法转换成类型：" + value.ToString() + "==>" + destinationType);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>" + destinationType, e);
            }
            return returnValue;
        }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Save(string path, string content)
        {
            string dir = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(path, content);
            return path;
        }
    }
}
