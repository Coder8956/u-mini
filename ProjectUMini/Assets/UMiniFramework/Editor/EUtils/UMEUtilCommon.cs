using System.IO;
using UMiniFramework.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.EUtils
{
    public class UMEUtilCommon
    {
        /// <summary>
        /// 判断路径是否包含 Application.dataPath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsContainsDataPath(string path)
        {
            // UMUtilDebug.Log($"IsContainsDataPath:{Application.dataPath}");
            return path.Contains(Application.dataPath);
        }

        /// <summary>
        /// 获取 AssetData Path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAssetDataPath(string path)
        {
            return path.Replace(Application.dataPath, "Assets");
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
        /// 检查给定路径下的预制体是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckPrefabExists(string path)
        {
            // 使用 AssetDatabase.LoadAssetAtPath 来加载资源
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab != null;
        }
    }
}