﻿using System.IO;
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
        public static string FindFilePath(string fileName, string extension)
        {
            // 在项目中查找文件
            string[] guids = AssetDatabase.FindAssets(fileName);
            // string[] guids = AssetDatabase.FindAssets(fileName + " t:script");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (System.IO.Path.GetFileName(path) == $"{fileName}{extension}")
                {
                    return path;
                }
            }

            return null;
        }

        public static void OpenAssetScriptFile(string fileName)
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


        public static void OpenFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                EditorUtility.DisplayDialog("Invalid Folder", folderPath, "OK");
                return;
            }

            EditorUtility.RevealInFinder(folderPath);
        }

        public static void DrawDefaultInspectorExcept(UnityEditor.Editor editor, string[] propertyNames)
        {
            // 复制原有的序列化对象
            var prop = editor.serializedObject.GetIterator();
            bool enterChildren = true;

            // 遍历所有属性
            while (prop.NextVisible(true))
            {
                // 仅绘制可见属性，并排除指定的属性
                if (System.Array.IndexOf(propertyNames, prop.name) == -1)
                {
                    EditorGUILayout.PropertyField(prop, true);
                }

                enterChildren = false;
            }

            // 只排除一个属性的代码
            // // 复制原有的序列化对象
            // var prop = serializedObject.GetIterator();
            // bool enterChildren = true;
            //
            // // 遍历所有属性
            // while (prop.NextVisible(enterChildren))
            // {
            //     // 仅绘制可见属性，并排除指定的属性
            //     if (prop.name != propertyName)
            //     {
            //         EditorGUILayout.PropertyField(prop, true);
            //     }
            //     enterChildren = false;
            // }
        }
    }
}