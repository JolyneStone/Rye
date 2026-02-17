using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rye.Util
{
    /// <summary>
    /// 对象属性级对比
    /// </summary>
    public class FieldDifferenceUtil
    {
        /// <summary>
        /// 获取对比结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static List<FieldDifference> GetDifferences<T>(T obj1, T obj2)
            where T : class
        {
            var differences = new List<FieldDifference>();

            if (obj1 == null || obj2 == null)
            {
                if (obj1 != obj2)
                {
                    differences.Add(new FieldDifference
                    {
                        Name = "Object",
                        Value1 = obj1,
                        Value2 = obj2,
                        Type = typeof(T)
                    });
                }
                return differences;
            }

            Type type = typeof(T);

            foreach (var member in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => p.CanRead))
            {
                var value1 = member.GetValue(obj1);
                var value2 = member.GetValue(obj2);

                if (ValuesDifferent(member.PropertyType, value1, value2))
                {
                    differences.Add(new FieldDifference
                    {
                        Name = member.Name,
                        Value1 = value1,
                        Value2 = value2,
                        Type = member.PropertyType
                    });
                }
            }

            return differences;
        }

        private static bool ValuesDifferent(Type type, object value1, object value2)
        {
            if (value1 == null && value2 == null) return false;
            if (value1 == null && value2 != null) return false;
            if (value1 != null && value2 == null) return false;
            if (value1 != null && value2 != null)
            {
                // 对于值类型，直接比较
                if (type.IsValueType || value1 is string)
                {
                    return !Equals(value1, value2);
                }

                if (value1.GetType() != value2.GetType())
                {
                    return true;
                }

                type = Nullable.GetUnderlyingType(type) ?? type;

                // 如果是可枚举类型（如数组、List等），只比较长度，不比较元素
                if (value1 is System.Collections.IEnumerable enum1 && value2 is System.Collections.IEnumerable enum2)
                {
                    var list1 = ((System.Collections.IEnumerable)value1).Cast<object>().ToList();
                    var list2 = ((System.Collections.IEnumerable)value2).Cast<object>().ToList();

                    if (list1.Count != list2.Count) return true;
                    if (list1.ToJsonString() != list2.ToJsonString()) return true;

                    return false;
                }

                // 对于普通引用类型，递归比较其公共字段和属性
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(p => p.CanRead);

                foreach (var prop in properties)
                {
                    var propValue1 = prop.GetValue(value1);
                    var propValue2 = prop.GetValue(value2);
                    if (ValuesDifferent(prop.GetType().GetTypeInfo(), propValue1, propValue2)) return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 对比结果
    /// </summary>
    public class FieldDifference
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Value1 { get; set; }
        public object Value2 { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }
    }
}
