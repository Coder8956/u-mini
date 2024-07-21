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

        public static void OpenScriptFile(string fileName)
        {
            string filePath = FindFilePath(fileName, ".cs");
            if (string.IsNullOrEmpty(filePath))
            {
                EditorUtility.DisplayDialog("File Not Found", "Cannot find the file: " + fileName, "OK");
                return;
            }

            // 加载脚本资产
            Object scriptAsset = AssetDatabase.LoadAssetAtPath<Object>(filePath);

            if (scriptAsset != null)
            {
                // 打开指定脚本
                AssetDatabase.OpenAsset(scriptAsset);
            }
            else
            {
                // 如果脚本路径不正确，显示错误信息
                EditorUtility.DisplayDialog("Error", "Cannot find the script at the specified path: " + filePath,
                    "OK");
            }
        }
    }
}