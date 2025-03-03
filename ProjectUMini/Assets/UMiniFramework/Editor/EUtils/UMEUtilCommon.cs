using System;
using System.Reflection;
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

        /// <summary>
        /// 获取对象的非公共方法
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public static MethodInfo GetObjectNoPublicMethod(Type type, string methodName)
        {
            return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// 获取对象的非公共字段(变量)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static FieldInfo GetObjectNoPublicField(Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}