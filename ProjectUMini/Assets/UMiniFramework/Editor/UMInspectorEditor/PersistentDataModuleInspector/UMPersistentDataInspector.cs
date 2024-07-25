using UMiniFramework.Editor.Common;
using UMiniFramework.Scripts.Modules.PersistentDataModule;
using UMiniFramework.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMInspectorEditor.PersistentDataModuleInspector
{
    [CustomEditor(typeof(UMPersistentDataModule))]
    public class UMPersistentDataInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawModifySaveDirPath();
        }


        private void DrawModifySaveDirPath()
        {
            if (GUILayout.Button("Modify PersistentData Save Dir"))
            {
                string fileName = "UMPersistentDataRootDir"; // 修改为你要查找的文件名
                UMEditorUtils.OpenAssetScriptFile(fileName);
            }

            if (GUILayout.Button("Open PersistentData Dir"))
            {
                string folderPath = UMPersistentDataRootDir.GetRootDir(); // 这里替换为你想打开的文件夹路径
                UMEditorUtils.OpenFolder(folderPath);
            }
        }
    }
}