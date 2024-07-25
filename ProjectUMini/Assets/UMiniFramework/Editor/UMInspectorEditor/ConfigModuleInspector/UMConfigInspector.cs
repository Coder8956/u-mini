using System;
using System.IO;
using UMiniFramework.Editor.Common;
using UMiniFramework.Editor.Const;
using UMiniFramework.Scripts.Modules.ConfigModule;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMInspectorEditor.ConfigModuleInspector
{
    [CustomEditor(typeof(UMConfigModule))]
    public class UMConfigInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawOpenExcelFolder();
            EditorGUILayout.Space(5);
            DrawOpenDataFolder();
            EditorGUILayout.Space(5);
            DrawOpenScriptFolder();
            EditorGUILayout.Space(5);
            DrawModifyConfigPath();
            DrawUpdateConfig();
        }

        private void DrawUpdateConfig()
        {
            if (GUILayout.Button("Update Config"))
            {
                UMConfigHandler.UpdateConfig(UMConfigPathConst.EXCELS_DIR, UMConfigPathConst.SCRIPTS_DIR,
                    UMConfigPathConst.DATA_DIR);
            }
        }

        private void DrawModifyConfigPath()
        {
            if (GUILayout.Button("Modify Config Path"))
            {
                string fileName = "UMConfigPathConst"; // 修改为你要查找的文件名
                UMEditorUtils.OpenAssetScriptFile(fileName);
            }
        }

        private void DrawOpenExcelFolder()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            // 保存原始颜色
            Color originalColor = GUI.color;
            string InvalidFolderTip = string.Empty;
            if (!Directory.Exists(UMConfigPathConst.EXCELS_DIR))
            {
                // 设置新颜色
                GUI.color = Color.red;
                InvalidFolderTip = "Invalid Folder";
            }

            EditorGUILayout.LabelField($"[Excel Folder] {InvalidFolderTip}");
            // 恢复原始颜色
            GUI.color = originalColor;
            EditorGUILayout.LabelField($"{UMConfigPathConst.EXCELS_DIR}");
            if (GUILayout.Button("Open Excel Folder"))
            {
                UMEditorUtils.OpenFolder(UMConfigPathConst.EXCELS_DIR);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawOpenScriptFolder()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            // 保存原始颜色
            Color originalColor = GUI.color;
            string InvalidFolderTip = string.Empty;
            if (!Directory.Exists(UMConfigPathConst.SCRIPTS_DIR))
            {
                // 设置新颜色
                GUI.color = Color.red;
                InvalidFolderTip = "Invalid Folder";
            }

            EditorGUILayout.LabelField($"[Script Folder] {InvalidFolderTip}");
            // 恢复原始颜色
            GUI.color = originalColor;
            EditorGUILayout.LabelField($"{UMConfigPathConst.SCRIPTS_DIR}");
            if (GUILayout.Button("Open Script Folder"))
            {
                UMEditorUtils.OpenFolder(UMConfigPathConst.SCRIPTS_DIR);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawOpenDataFolder()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            // 保存原始颜色
            Color originalColor = GUI.color;
            string InvalidFolderTip = string.Empty;
            if (!Directory.Exists(UMConfigPathConst.DATA_DIR))
            {
                // 设置新颜色
                GUI.color = Color.red;
                InvalidFolderTip = "Invalid Folder";
            }

            EditorGUILayout.LabelField($"[Data Folder] {InvalidFolderTip}");
            // 恢复原始颜色
            GUI.color = originalColor;
            EditorGUILayout.LabelField($"{UMConfigPathConst.DATA_DIR}");
            if (GUILayout.Button("Open Data Folder"))
            {
                UMEditorUtils.OpenFolder(UMConfigPathConst.DATA_DIR);
            }

            EditorGUILayout.EndVertical();
        }
    }
}