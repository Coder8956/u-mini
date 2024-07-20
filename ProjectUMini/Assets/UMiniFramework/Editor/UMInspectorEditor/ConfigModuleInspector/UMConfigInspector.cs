using System;
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
                string filePath = UMEditorUtils.FindFilePath(fileName, ".cs");
                if (string.IsNullOrEmpty(filePath))
                {
                    EditorUtility.DisplayDialog("File Not Found", "Cannot find the file: " + fileName, "OK");
                    return;
                }

                // 加载脚本资产
                UnityEngine.Object scriptAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath);

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
}