using System.IO;
using System.Text;

namespace UMiniFramework.Scripts.Utils
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
    }
}