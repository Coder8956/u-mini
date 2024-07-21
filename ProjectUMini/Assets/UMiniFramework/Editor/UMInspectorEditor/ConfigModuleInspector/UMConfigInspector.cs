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
                UMEditorUtils.OpenScriptFile(fileName);
            }
        }
    }
}