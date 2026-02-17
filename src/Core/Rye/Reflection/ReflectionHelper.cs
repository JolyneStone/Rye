using Rye.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Reflection
{
    /// <summary>
    /// 对象反射帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        #region 获得项目关联库文件

        /// <summary>
        /// 获得项目关联库文件
        /// </summary>
        /// <param name="startsWith">开头文件名</param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(params string[] startsWith)
        {
            var package = App.Configuration.GetSection<List<string>>("AppSettings:SupportPackageNamePrefixs");
            var dirPath = DirectoryUtil.BaseDirectory();
            var files = new List<FileInfo>();
            startsWith = startsWith ?? [];
            foreach (var sw in package == null ?
                                startsWith :
                                startsWith.Concat(package))
            {
                var files1 = FileUtil.GetFiles(dirPath, sw, ".dll");
                if (files1 != null && files1.Length > 0)
                {
                    files.AddRange(files1);
                }
            }
            return files.ToArray();
        }

        #endregion 获得项目关联库文件

        #region 反射Type类型

        /// <summary>
        /// 反射Type类型
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Type GetType(string nameSpace, string className)
        {
            foreach (var file in GetFiles())
            {
                var assemblyName = file.Name.Replace(".dll", "");
                var assembly = Assembly.Load(assemblyName);
                //加载类型
                var type = assembly.GetTypes().Where(t => t.Namespace == nameSpace && t.Name == className).FirstOrDefault();
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        #endregion 反射Type类型

        #region 创建对象实例

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string nameSpace, string className, string assemblyName) where T : class
        {
            try
            {
                //命名空间.类名,程序集
                string path = nameSpace + "." + className + "," + assemblyName;
                //加载类型
                Type type = Type.GetType(path);
                //根据类型创建实例
                object obj = Activator.CreateInstance(type, true);
                //类型转换并返回
                return (T)obj;
            }
            catch
            {
                //发生异常时，返回类型的默认值。
                return default;
            }
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static object CreateInstance(string nameSpace, string className)
        {
            foreach (var file in GetFiles())
            {
                var assemblyName = file.Name.Replace(".dll", "");
                var assembly = Assembly.Load(assemblyName);
                //加载类型
                var type = assembly.GetTypes().Where(t => t.Namespace == nameSpace && t.Name == className).FirstOrDefault();
                if (type != null)
                {
                    //根据类型创建实例
                    object obj = Activator.CreateInstance(type, true);
                    //类型转换并返回
                    return obj;
                }
            }
            return default;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string nameSpace, string className) where T : class
        {
            return (T)CreateInstance(nameSpace, className);
        }

        #endregion 创建对象实例

        #region 调用方法实例

        /// <summary>
        /// 调用方法实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paras">参数集合</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static T GetInvokeMethod<T>(string nameSpace, string className, string methodName, object[] paras, string assemblyName)
        {
            try
            {
                //命名空间.类名,程序集
                string path = nameSpace + "." + className + "," + assemblyName;
                //加载类型
                Type type = Type.GetType(path);
                //根据类型创建实例
                object obj = Activator.CreateInstance(type, true);
                //加载方法参数类型及方法
                MethodInfo method = null;
                if (paras != null && paras.Length > 0)
                {
                    //加载方法参数类型
                    Type[] paratypes = new Type[paras.Length];
                    for (int i = 0; i < paras.Length; i++)
                    {
                        paratypes[i] = paras[i].GetType();
                    }
                    //加载有参方法
                    method = type.GetMethod(methodName, paratypes);
                }
                else
                {
                    //加载无参方法
                    method = type.GetMethod(methodName);
                }
                //类型转换并返回
                return (T)method.Invoke(obj, paras);
            }
            catch
            {
                //发生异常时，返回类型的默认值。
                return default;
            }
        }

        /// <summary>
        /// 调用方法实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public static T GetInvokeMethod<T>(string nameSpace, string className, string methodName, object[] paras)
        {
            foreach (var file in GetFiles())
            {
                var assemblyName = file.Name.Replace(".dll", "");
                var assembly = Assembly.Load(assemblyName);
                //加载类型
                var type = assembly.GetTypes().Where(t => t.Namespace == nameSpace && t.Name == className).FirstOrDefault();
                if (type != null)
                {
                    //根据类型创建实例
                    object obj = Activator.CreateInstance(type, true);
                    //加载方法参数类型及方法
                    MethodInfo method = null;
                    if (paras != null && paras.Length > 0)
                    {
                        //加载方法参数类型
                        Type[] paratypes = new Type[paras.Length];
                        for (int i = 0; i < paras.Length; i++)
                        {
                            paratypes[i] = paras[i].GetType();
                        }
                        //加载有参方法
                        method = type.GetMethod(methodName, paratypes);
                    }
                    else
                    {
                        //加载无参方法
                        method = type.GetMethod(methodName);
                    }
                    //类型转换并返回
                    //类型转换并返回
                    if (method == null)
                    {
                        return default;
                    }
                    return (T)method.Invoke(obj, paras);
                }
            }
            return default;
        }

        /// <summary>
        /// 异步调用方法实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public static async Task<T> GetInvokeMethodAsync<T>(string nameSpace, string className, string methodName, object[] paras)
        {
            foreach (var file in GetFiles())
            {
                var assemblyName = file.Name.Replace(".dll", "");
                var assembly = Assembly.Load(assemblyName);
                //加载类型
                var type = assembly.GetTypes().Where(t => t.Namespace == nameSpace && t.Name == className).FirstOrDefault();
                if (type != null)
                {
                    //根据类型创建实例
                    object obj = Activator.CreateInstance(type, true);
                    //加载方法参数类型及方法
                    MethodInfo method = null;
                    if (paras != null && paras.Length > 0)
                    {
                        //加载方法参数类型
                        Type[] paratypes = new Type[paras.Length];
                        for (int i = 0; i < paras.Length; i++)
                        {
                            paratypes[i] = paras[i].GetType();
                        }
                        //加载有参方法
                        method = type.GetMethod(methodName, paratypes);
                    }
                    else
                    {
                        //加载无参方法
                        method = type.GetMethod(methodName);
                    }
                    //类型转换并返回
                    if (method == null)
                    {
                        return default;
                    }
                    return await (Task<T>)method.Invoke(obj, paras);
                }
            }
            return default;
        }

        #endregion 调用方法实例
    }
}
