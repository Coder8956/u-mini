using System;
using UMiniFramework.Editor.Common;
using UMiniFramework.Editor.Const;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMWindows.ConfigWindow
{
    public class UMConfigWindow : EditorWindow
    {
        [MenuItem(UMEditorConst.UMEDITOR_WINDOWS_TITLE_ROOT + "/Config")]
        private static void ShowWindow()
        {
            var window = GetWindow<UMConfigWindow>();
            window.titleContent = new GUIContent("UMConfigWindow");
            window.Show();
            window.maxSize = new Vector2(window.maxSize.x, 100);
        }

        private string m_excelsDir = string.Empty;
        private string m_jsonDir = string.Empty;
        private string m_scriptsDir = string.Empty;
        private string m_selectDir = string.Empty;

        private void OnEnable()
        {
            // 初始化代码
            m_excelsDir = GetDirVal(UMConfigWindowConst.KEY_EXCELS_DIR);
            m_jsonDir = GetDirVal(UMConfigWindowConst.KEY_JSON_DIR);
            m_scriptsDir = GetDirVal(UMConfigWindowConst.KEY_SCRIPTS_DIR);
        }

        private void OnGUI()
        {
            DrawExcelConfigDirectory();
            DrawJsonFilesOutputDirectory();
            DrawConfigScriptsOutputDirectory();
            DrawUpdateConfig();
        }

        private void DrawExcelConfigDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(UMConfigWindowConst.EXCEL_CONFIG_DIRECTORY, EditorStyles.textField,
                GUILayout.Width(150));
            EditorGUILayout.LabelField(m_excelsDir, EditorStyles.textField);
            if (GUILayout.Button(UMConfigWindowConst.SELECT_DIRECTORY, GUILayout.Width(140)))
            {
                m_selectDir =
                    EditorUtility.OpenFolderPanel(UMConfigWindowConst.EXCEL_CONFIG_DIRECTORY, m_excelsDir, "");
                if (m_selectDir != String.Empty && m_selectDir != m_excelsDir)
                {
                    m_excelsDir = m_selectDir;
                    SaveDirVal(UMConfigWindowConst.KEY_EXCELS_DIR, m_excelsDir);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawJsonFilesOutputDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(UMConfigWindowConst.JSON_FILES_OUTPUT_DIRECTORY, EditorStyles.textField,
                GUILayout.Width(150));
            EditorGUILayout.LabelField(m_jsonDir, EditorStyles.textField);
            if (GUILayout.Button(UMConfigWindowConst.SELECT_DIRECTORY, GUILayout.Width(140)))
            {
                m_selectDir =
                    EditorUtility.OpenFolderPanel(UMConfigWindowConst.JSON_FILES_OUTPUT_DIRECTORY, m_jsonDir, "");
                if (m_selectDir != String.Empty && m_selectDir != m_jsonDir)
                {
                    m_jsonDir = m_selectDir;
                    SaveDirVal(UMConfigWindowConst.KEY_JSON_DIR, m_jsonDir);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawConfigScriptsOutputDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(UMConfigWindowConst.CONFIG_SCRIPTS_OUTPUT_DIRECTORY, EditorStyles.textField,
                GUILayout.Width(150));
            EditorGUILayout.LabelField(m_scriptsDir, EditorStyles.textField);
            if (GUILayout.Button(UMConfigWindowConst.SELECT_DIRECTORY, GUILayout.Width(140)))
            {
                m_selectDir =
                    EditorUtility.OpenFolderPanel(UMConfigWindowConst.CONFIG_SCRIPTS_OUTPUT_DIRECTORY, m_scriptsDir,
                        "");
                if (m_selectDir != String.Empty && m_selectDir != m_scriptsDir)
                {
                    m_scriptsDir = m_selectDir;
                    SaveDirVal(UMConfigWindowConst.KEY_SCRIPTS_DIR, m_scriptsDir);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawUpdateConfig()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(UMConfigWindowConst.UPDATE_CONFIG))
            {
            }

            EditorGUILayout.EndHorizontal();
        }

        private string GetDirVal(string key)
        {
            string val = EditorPrefs.GetString(key, string.Empty);
            if (val == String.Empty)
            {
                val = UMEditorUtils.GetProjectPath();
            }

            return val;
        }

        private void SaveDirVal(string key, string val)
        {
            EditorPrefs.SetString(key, val);
        }
    }
}