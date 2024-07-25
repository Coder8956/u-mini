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
            DrawOpenExcelFolder();
            DrawOpenScriptFolder();
            DrawOpenDataFolder();
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
            if (GUILayout.Button("Open Excel Folder"))
            {
                UMEditorUtils.OpenFolder(UMConfigPathConst.EXCELS_DIR);
            }
        }

        private void DrawOpenScriptFolder()
        {
            if (GUILayout.Button("Open Script Folder"))
            {
                UMEditorUtils.OpenFolder(UMConfigPathConst.SCRIPTS_DIR);
            }
        }

        private void DrawOpenDataFolder()
        {
            if (GUILayout.Button("Open Data Folder"))
            {
                UMEditorUtils.OpenFolder(UMConfigPathConst.DATA_DIR);
            }
        }
    }
}