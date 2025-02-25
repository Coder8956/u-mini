using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.Config
{
    public class UMConfigEditorWindow : EditorWindow
    {
        private const string GUI_STYLE_HELPBOX = "HelpBox";
        private string m_configInputDir = string.Empty;
        private string m_configScriptOutputDir = string.Empty;
        private string m_configJsonOutputDir = string.Empty;

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

            GUILayout.Label("Config Input Directory", GUILayout.Width(130), GUILayout.Height(layoutHeight));

            if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 打开文件夹选择框
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    m_configInputDir = selectedPath;
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 清除路径
                m_configInputDir = string.Empty;
            }

            GUILayout.Label(m_configInputDir, GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));

            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSelectConfigScriptOutputFolder()
        {
            EditorGUILayout.BeginHorizontal();

            int layoutHeight = 20;

            GUILayout.Label("Config Script Output Directory", GUILayout.Width(180), GUILayout.Height(layoutHeight));

            if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 打开文件夹选择框
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    m_configScriptOutputDir = selectedPath;
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 清除路径
                m_configScriptOutputDir = string.Empty;
            }

            GUILayout.Label(m_configScriptOutputDir, GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));

            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSelectConfigJsonOutputFolder()
        {
            EditorGUILayout.BeginHorizontal();

            int layoutHeight = 20;

            GUILayout.Label("Config Json Output Directory", GUILayout.Width(180), GUILayout.Height(layoutHeight));

            if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 打开文件夹选择框
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    m_configJsonOutputDir = selectedPath;
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 清除路径
                m_configJsonOutputDir = string.Empty;
            }

            GUILayout.Label(m_configJsonOutputDir, GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));

            EditorGUILayout.EndHorizontal();
        }
        
        private void OnGUI()
        {
            DrawSelectConfigInputFolder();
            DrawSelectConfigScriptOutputFolder();
            DrawSelectConfigJsonOutputFolder();
        }
    }
}