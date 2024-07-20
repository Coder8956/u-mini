using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.Common
{
    public class UMEditorUtils
    {
        public static string GetProjectPath()
        {
            string dataPath = Application.dataPath;
            string projectPath = dataPath.Substring(0, dataPath.LastIndexOf("/Assets"));
            return projectPath;
        }

        /// <summary>
        /// 查找文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindFilePath(string fileName, string fileNameExtension)
        {
            // 在项目中查找文件
            string[] guids = AssetDatabase.FindAssets(fileName);
            // string[] guids = AssetDatabase.FindAssets(fileName + " t:script");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (System.IO.Path.GetFileName(path) == $"{fileName}{fileNameExtension}")
                {
                    return path;
                }
            }

            return null;
        }
    }
}