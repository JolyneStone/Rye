using System;
using System.IO;

namespace Rye.Util
{
    /// <summary>
    /// 文件夹操作帮助类
    /// </summary>
    public class DirectoryUtil
    {
        /// <summary>
        /// 应用根目录
        /// </summary>
        /// <returns></returns>
        public static string BaseDirectory()
        {
            return AppContext.BaseDirectory;
        }

        /// <summary>
        /// 文件夹路径获取
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static string DirPath(string dirPath)
        {
            //设置绝对路径
            if (string.IsNullOrWhiteSpace(dirPath))
            {
                dirPath = AppContext.BaseDirectory;
            }
            else if (!dirPath.Contains('/') && !dirPath.Contains('\\'))
            {
                dirPath = Path.Combine(AppContext.BaseDirectory, dirPath);
            }
            else if (!dirPath.Contains(":\\"))
            {
                dirPath = Path.Combine(AppContext.BaseDirectory, dirPath);
            }
            return dirPath;
        }

        /// <summary>
        /// 文件夹路径切割
        /// </summary>
        /// <param name="dirPaht"></param>
        /// <returns></returns>
        public static string Cut(string dirPaht)
        {
            if (!string.IsNullOrEmpty(dirPaht))
            {
                var fileName = Path.GetFileName(dirPaht);
                if (!string.IsNullOrEmpty(fileName))
                {
                    dirPaht = dirPaht.SubstringLeft(dirPaht.Length - fileName.Length);
                }
            }
            return dirPaht;
        }

        /// <summary>
        /// 文件夹目录创建
        /// </summary>
        /// <param name="dirPath">路径</param>
        public static void Create(string dirPath)
        {
            dirPath = Cut(dirPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// 文件夹目录删除
        /// </summary>
        /// <param name="dirPath">目录地址</param>
        public static void Delete(string dirPath)
        {
            dirPath = Cut(dirPath);
            if (Directory.Exists(dirPath))
            {
                string[] fileSystemEntries = Directory.GetFileSystemEntries(dirPath);
                foreach (var ft in fileSystemEntries)
                {
                    if (File.Exists(ft))
                    {
                        File.Delete(ft);
                    }
                }
                Directory.Delete(dirPath);
            }
        }
    }
}