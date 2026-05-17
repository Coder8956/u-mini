using System.IO;
using System.Text;

namespace UMiniFramework.Runtime.Utils
{
    public static class UMUtilIO
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file"></param>
        public static bool IsExistsFile(string file)
        {
            return File.Exists(file);
        }

        public static string FileReadAllText(string file)
        {
            return File.ReadAllText(file);
        }

        public static void FileWriteAllText(string path, string content)
        {
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        /// <summary>
        /// 格式化路径分割符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPathSeparator(string path)
        {
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// 清空目录
        /// </summary>
        /// <param name="dirPath"></param>
        public static void ClearDir(string dirPath)
        {
            string directoryPath = dirPath; // 你要清空的目录路径

            // 获取目录中的所有文件和子目录
            string[] files = Directory.GetFiles(directoryPath);
            string[] directories = Directory.GetDirectories(directoryPath);

            // 删除文件
            foreach (var file in files)
            {
                File.Delete(file);
            }

            // 删除子目录
            foreach (var dir in directories)
            {
                Directory.Delete(dir, true);
            }
        }
    }
}