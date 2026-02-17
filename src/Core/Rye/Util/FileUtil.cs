using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rye.Util
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 文件是否存在判断
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool IsExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 文件是否存在判断
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns></returns>
        public static bool IsExists(FileInfo fileInfo)
        {
            return IsExists(fileInfo.FullName);
        }

        /// <summary>
        /// 获得目录下的所有文件
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string rootPath)
        {
            DirectoryInfo root = new(rootPath);
            return root.GetFiles();
        }

        /// <summary>
        /// 获得目录下的所有文件
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="filterFiles"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string rootPath, List<string> filterFiles)
        {
            var files = GetFiles(rootPath);
            return files.Where(w => filterFiles.Contains(w.Name)).ToArray();
        }

        /// <summary>
        /// 获得指定前后缀的文件集合
        /// </summary>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string rootPath, string startsWith, string endsWith)
        {
            var files = GetFiles(rootPath);
            return files.Where(w => w.Name.StartsWith(startsWith) && w.Name.EndsWith(endsWith)).ToArray();
        }

        /// <summary>
        /// 文件写入
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="path"></param>
        public static void Write(StringBuilder sb, string path)
        {
            DirectoryUtil.Create(path);

            var filename = $"{path}/{path}";
            //不包含BOM格式的UTF8
            var utf8 = new UTF8Encoding(false);
            StreamWriter sw = new(filename, false, utf8);
            sw.Write(sb);
            sw.Close();
        }
    }
}