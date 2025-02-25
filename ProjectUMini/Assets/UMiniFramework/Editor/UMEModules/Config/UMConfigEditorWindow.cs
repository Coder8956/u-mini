using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.Config
{
    public class UMConfigEditorWindow : EditorWindow
    {
        [MenuItem("UMUtils/Config/Config-Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<UMConfigEditorWindow>();
            window.titleContent = new GUIContent("UMConfig Editor");
            window.Show();
        }

        private void DrawSelectConfigInputFolder()
        {
            EditorGUILayout.BeginHorizontal();

            int layoutHeight = 20;

            // GUILayout.Label("Select UIPanel Prefab Folder:", EditorStyles.boldLabel);
            GUILayout.Label("UI Prefab Root Folder", GUILayout.Width(125), GUILayout.Height(layoutHeight));

            if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 打开文件夹选择框
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (!selectedPath.Contains(Application.dataPath))
                    {
                        EditorUtility.DisplayDialog("Tip",
                            $"The selected folder is invalid. Please select a folder inside path {Application.dataPath}",
                            "OK");
                    }
                    else
                    {
                    }
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 清除路径
            }

            // GUILayout.Label("Selected Folder Path: ", GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));
            // GUILayout.Label(m_panelPrefabRootFolder, GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));

            EditorGUILayout.EndHorizontal();
        }

        private void OnGUI()
        {
            DrawSelectConfigInputFolder();
        }
    }
}