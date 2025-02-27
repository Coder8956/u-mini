using System;
using System.Collections.Generic;
using UMiniFramework.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.Config
{
    public class UMConfigEditorWindow : EditorWindow
    {
        private const string READ_FILSE_TIP_1 = "Please click read button";
        private const string READ_FILSE_TIP_2 = "The configuration file was not read";
        private const string READ_FILSE_TIP_3 = "{0} configuration files were read";
        private const string GUI_STYLE_HELPBOX = "HelpBox";
        private string m_configInputDir = string.Empty;
        private string m_configScriptOutputDir = string.Empty;
        private string m_readFilseTip = string.Empty;
        private string m_configJsonOutputDir = string.Empty;
        private GUIStyle m_redLabelStyle;

        // 滚动视图位置
        private Vector2 m_scrollPosition;
        private float m_scrollViewHeight;

        private List<string> m_configFiles;

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
                string selectedPath =
                    EditorUtility.OpenFolderPanel("Select Config Input Folder", Application.dataPath, "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    m_configInputDir = selectedPath;
                    m_readFilseTip = READ_FILSE_TIP_1;
                    m_configFiles.Clear();
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 清除路径
                m_configInputDir = string.Empty;
                m_configFiles.Clear();
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
                string selectedPath =
                    EditorUtility.OpenFolderPanel("Select Script Output Folder", Application.dataPath, "");

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
                string selectedPath =
                    EditorUtility.OpenFolderPanel("Select Json Output Folder", Application.dataPath, "");

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

        private void DrawOutputDirClearWarning()
        {
            GUILayout.Label(
                "The output directory is cleared when the [Update Config] is performed.",
                m_redLabelStyle);
        }

        private void DrawCreateConfig()
        {
            if (m_configJsonOutputDir == String.Empty || m_configScriptOutputDir == String.Empty) return;
            if (GUILayout.Button("Update Config"))
            {
                UMConfigHandler.UpdateConfig(m_configFiles, m_configScriptOutputDir, m_configJsonOutputDir);
            }
        }

        private void DrawReadConfigFiles()
        {
            if (m_configInputDir == string.Empty) return;

            if (GUILayout.Button("Read Config Files"))
            {
                m_configFiles.Clear();
                UMConfigHandler.GetAllExcelFiles(m_configInputDir, m_configFiles);

                m_scrollViewHeight = m_configFiles.Count * 20;

                if (m_configFiles.Count < 1)
                {
                    m_readFilseTip = READ_FILSE_TIP_2;
                }
                else
                {
                    m_readFilseTip = string.Format(READ_FILSE_TIP_3, m_configFiles.Count);
                }

                m_scrollViewHeight = Mathf.Clamp(m_scrollViewHeight, 0, 400);
            }

            GUILayout.Label(m_readFilseTip, GUI_STYLE_HELPBOX, GUILayout.Height(20));

            if (m_configFiles.Count < 1) return;
            // 创建滚动区域
            m_scrollPosition =
                GUILayout.BeginScrollView(m_scrollPosition, GUILayout.Width(position.width),
                    GUILayout.Height(m_scrollViewHeight));

            // 绘制内容
            for (int i = 0; i < m_configFiles.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Config-{string.Format("{0:D5}", i)}");
                GUILayout.Space(5);
                GUILayout.Label(m_configFiles[i]);
                GUILayout.FlexibleSpace();
                // GUILayout.Label(m_configFiles[i]);
                GUILayout.EndHorizontal();
            }

            // 结束滚动区域
            GUILayout.EndScrollView();
        }

        private void OnEnable()
        {
            if (m_configFiles == null)
            {
                m_configFiles = new List<string>();
            }
        }

        private void OnGUI()
        {
            if (m_redLabelStyle == null)
            {
                // 创建一个新的 GUIStyle
                m_redLabelStyle = new GUIStyle(GUI.skin.label);
                // 设置字体颜色为红色
                m_redLabelStyle.normal.textColor = Color.red;
            }

            DrawSelectConfigInputFolder();
            DrawReadConfigFiles();
            GUI.enabled = m_configFiles.Count > 0;
            DrawSelectConfigScriptOutputFolder();
            DrawSelectConfigJsonOutputFolder();
            DrawOutputDirClearWarning();
            DrawCreateConfig();
        }
    }
}