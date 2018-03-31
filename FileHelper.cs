using System;
using System.Collections.Generic;
using System.IO;

namespace NodeEditor.Utils
{
    public static class FileHelper
    {
        public static void WriteLines(List<string> lines, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);

            string result = "";

            foreach (var line in lines)
            {
                result += line + "\n";
            }

            byte[] data = System.Text.Encoding.UTF8.GetBytes(result);

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }

        public static void Write(string content, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);

            byte[] data = System.Text.Encoding.UTF8.GetBytes(content);

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }

        public static string Read(string path)
        {
            string content = string.Empty;
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                NodeDebug.LogError("FileHelper Read file error: " + e);
            }

            return content;
        }

        public static string[] ReadLines(string path)
        {
            string[] contents = null;
            try
            {
                contents = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                NodeDebug.LogError("FileHelper Read file all lines error: " + e);
            }
            return contents;
        }

        /// <summary>
        /// 得到目录下的所有文件信息
        /// </summary>
        /// <returns>所有文件信息</returns>
        /// <param name="path">目录</param>
        /// <param name="suffix">文件后缀</param>
        /// <param name="recursive">是否递归遍历</param>
        public static List<FileInfo> GetFiles(string path, string suffix, bool recursive = false)
        {
            var dir = new DirectoryInfo(path);
            if (dir == null)
                return null;

            List<FileInfo> ret = new List<FileInfo>();

            InnerGetFiles(dir, ref ret, suffix, recursive);

            return ret;
        }

        private static void InnerGetFiles(DirectoryInfo root, ref List<FileInfo> fileList, string suffix, bool recursive = false)
        {
            var files = root.GetFiles(suffix);
            fileList.AddRange(files);

            if (recursive)
            {
                var dirs = root.GetDirectories();
                foreach (var dir in dirs)
                {
                    InnerGetFiles(dir, ref fileList, suffix, recursive);
                }
            }
        }

        /// <summary>
        /// 删除目录下的所有文件
        /// </summary>
        /// <returns>如果有删除的文件则返回真，否则返回假</returns>
        /// <param name="path">路径</param>
        /// <param name="recursive">是否递归删除</param>
        public static bool DelFiles(string path, string suffix, bool recursive = false)
        {
            var files = GetFiles(path, suffix, recursive);
            if (files == null || files.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (var file in files)
                {
                    File.Delete(file.FullName);
                }

                return true;
            }
        }
    }
}