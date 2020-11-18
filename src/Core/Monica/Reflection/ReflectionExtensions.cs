using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Monica.Reflection
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 判断当前类型是否可由指定类型派生
        /// </summary>
        public static bool IsDeriveClassFrom<TBaseType>(this Type type, bool canAbstract = false)
        {
            return IsDeriveClassFrom(type, typeof(TBaseType), canAbstract);
        }

        /// <summary>
        /// 判断当前类型是否可由指定类型派生
        /// </summary>
        public static bool IsDeriveClassFrom(this Type type, Type baseType, bool canAbstract = false)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(baseType, nameof(baseType));

            return type.IsClass && (canAbstract || !type.IsAbstract) && type.IsBaseOn(baseType);
        }

        /// <summary>
        /// 判断类型是否为Nullable类型
        /// </summary>
        /// <param name="type"> 要处理的类型 </param>
        /// <returns> 是返回True，不是返回False </returns>
        public static bool IsNullableType(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 由类型的Nullable类型返回实际类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetNonNullableType(this Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }

        /// <summary>
        /// 通过类型转换器获取Nullable类型的基础类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetUnNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                NullableConverter nullableConverter = new NullableConverter(type);
                return nullableConverter.UnderlyingType;
            }
            return type;
        }

        /// <summary>
        /// 获取类型的Description特性描述信息
        /// </summary>
        /// <param name="type">类型对象</param>
        /// <param name="inherit">是否搜索类型的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回类型的全名</returns>
        public static string GetDescription(this Type type, bool inherit = true)
        {
            DescriptionAttribute desc = type.GetAttribute<DescriptionAttribute>(inherit);
            return desc == null ? type.FullName : desc.Description;
        }

        /// <summary>
        /// 获取成员元数据的Description特性描述信息
        /// </summary>
        /// <param name="member">成员元数据对象</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string GetDescription(this MemberInfo member, bool inherit = true)
        {
            DescriptionAttribute desc = member.GetAttribute<DescriptionAttribute>(inherit);
            if (desc != null)
            {
                return desc.Description;
            }
            DisplayNameAttribute displayName = member.GetAttribute<DisplayNameAttribute>(inherit);
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            DisplayAttribute display = member.GetAttribute<DisplayAttribute>(inherit);
            if (display != null)
            {
                return display.Name;
            }
            return member.Name;
        }

        /// <summary>
        /// 检查指定指定类型成员中是否存在指定的Attribute特性
        /// </summary>
        /// <typeparam name="T">要检查的Attribute特性类型</typeparam>
        /// <param name="memberInfo">要检查的类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>是否存在</returns>
        public static bool HasAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.IsDefined(typeof(T), inherit);
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return attributes.FirstOrDefault() as T;
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>返回所有指定Attribute特性的数组</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Select(d=>d.Parse<T>()).ToArray();
        }

        /// <summary>
        /// 判断类型是否为集合类型
        /// </summary>
        /// <param name="type">要处理的类型</param>
        /// <returns>是返回True，不是返回False</returns>
        public static bool IsEnumerable(this Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// 判断当前泛型类型是否可由指定类型的实例填充
        /// </summary>
        /// <param name="genericType">泛型类型</param>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        public static bool IsGenericAssignableFrom(this Type genericType, Type type)
        {
            Check.NotNull(genericType, nameof(genericType));
            Check.NotNull(type, nameof(type));

            if (!genericType.IsGenericType)
            {
                throw new ArgumentException("该功能只支持泛型类型的调用，非泛型类型可使用 IsAssignableFrom 方法。");
            }

            List<Type> allOthers = new List<Type> { type };
            if (genericType.IsInterface)
            {
                allOthers.AddRange(type.GetInterfaces());
            }

            foreach (var other in allOthers)
            {
                Type cur = other;
                while (cur != null)
                {
                    if (cur.IsGenericType)
                    {
                        cur = cur.GetGenericTypeDefinition();
                    }
                    if (cur.IsSubclassOf(genericType) || cur == genericType)
                    {
                        return true;
                    }
                    cur = cur.BaseType;
                }
            }
            return false;
        }

        /// <summary>
        /// 方法是否是异步
        /// </summary>
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType == typeof(Task)
                || method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }

        /// <summary>
        /// 返回当前类型是否是指定基类的派生类
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <param name="baseType">要判断的基类型</param>
        /// <returns></returns>
        public static bool IsBaseOn(this Type type, Type baseType)
        {
            if (baseType.IsGenericTypeDefinition)
            {
                return baseType.IsGenericAssignableFrom(type);
            }
            return baseType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 返回当前类型是否是指定基类的派生类
        /// </summary>
        /// <typeparam name="TBaseType">要判断的基类型</typeparam>
        /// <param name="type">当前类型</param>
        /// <returns></returns>
        public static bool IsBaseOn<TBaseType>(this Type type)
        {
            Type baseType = typeof(TBaseType);
            return type.IsBaseOn(baseType);
        }

        /// <summary>
        /// 返回当前方法信息是否是重写方法
        /// </summary>
        /// <param name="method">要判断的方法信息</param>
        /// <returns>是否是重写方法</returns>
        public static bool IsOverridden(this MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }

        /// <summary>
        /// 返回当前属性信息是否为virtual
        /// </summary>
        public static bool IsVirtual(this PropertyInfo property)
        {
            var accessor = property.GetAccessors().FirstOrDefault();
            if (accessor == null)
            {
                return false;
            }

            return accessor.IsVirtual && !accessor.IsFinal;
        }

        /// <summary>
        /// 获取类型的全名，附带所在类库
        /// </summary>
        public static string GetFullNameWithModule(this Type type)
        {
            return $"{type.FullName},{type.Module.Name.Replace(".dll", "").Replace(".exe", "")}";
        }

        /// <summary>
        /// 判断目标类型的实例是否可为空
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TypeAllowsNullValue(this Type type)
        {
            return (!type.IsValueType || Nullable.GetUnderlyingType(type) != null);
        }

        /// <summary>
        /// 判断指定的类型是否是可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableValueType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// 判断指定的类型是否是基元类型
        /// 注：string不是基元类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPrimitiveType(this Type type)
        {
            return type.IsPrimitive;
        }

        /// <summary>
        /// 得到所实现的泛型接口
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static Type ExtractGenericInterface(this Type queryType, Type interfaceType)
        {
            if (queryType.IsGenericType &&
                queryType.GetGenericTypeDefinition() == interfaceType)
            {
                return queryType;
            }

            var queryTypeInterfaces = queryType.GetInterfaces();

            return queryTypeInterfaces.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }

        /// <summary>
        /// 判断某类型与其目标类型是否相匹配
        /// </summary>
        /// <param name="type"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool IsMatch(this Type type, Type targetType)
        {
            // case 1： targetType是开放类型
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == targetType)
            {
                return true;
            }

            // case 2：type是否是targetType的实现类型
            if (targetType.IsAssignableFrom(type))
            {
                return true;
            }

            // case 3：泛型接口的匹配
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取指定类型的C#类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetCSharpTypeName(this Type type)
        {
            if (type.IsGenericType)
            {
                var typeName = type.GetGenericTypeDefinition().FullName;
                typeName = typeName.Substring(0, typeName.IndexOf('`')) + '<';
                var generTypes = type.GetGenericArguments();
                typeName = typeName + GetCSharpTypeName(generTypes[0]);
                if (generTypes.Length > 1)
                {
                    for (int i = 1; i < generTypes.Length; i++)
                    {
                        typeName = typeName + ',' + GetCSharpTypeName(generTypes[i]);
                    }
                }

                typeName = typeName + ">";
                return typeName;
            }

            return type.FullName;

        }
    }
}
