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
        private string m_excelsDir = string.Empty;
        private string m_dataDir = string.Empty;
        private string m_scriptsDir = string.Empty;
        private string m_selectDir = string.Empty;

        private void OnEnable()
        {
            // 初始化代码
            m_excelsDir = GetDirVal(UMConfigInspectorConst.KEY_EXCELS_DIR);
            m_dataDir = GetDirVal(UMConfigInspectorConst.KEY_DATA_DIR);
            m_scriptsDir = GetDirVal(UMConfigInspectorConst.KEY_SCRIPTS_DIR);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawExcelConfigDirectory();
            GUILayout.Space(10);
            DrawJsonFilesOutputDirectory();
            GUILayout.Space(10);
            DrawConfigScriptsOutputDirectory();
            DrawUpdateConfig();
        }

        private void DrawExcelConfigDirectory()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(UMConfigInspectorConst.EXCEL_CONFIG_DIRECTORY, GUILayout.Width(150));
            EditorGUILayout.LabelField(m_excelsDir, EditorStyles.textField);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button(UMConfigInspectorConst.SELECT_EXCELS_DIRECTORY))
            {
                m_selectDir =
                    EditorUtility.OpenFolderPanel(UMConfigInspectorConst.EXCEL_CONFIG_DIRECTORY, m_excelsDir, "");
                if (m_selectDir != String.Empty && m_selectDir != m_excelsDir)
                {
                    m_excelsDir = m_selectDir;
                    SaveDirVal(UMConfigInspectorConst.KEY_EXCELS_DIR, m_excelsDir);
                }
            }
        }

        private void DrawJsonFilesOutputDirectory()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(UMConfigInspectorConst.DATA_FILES_OUTPUT_DIRECTORY, GUILayout.Width(150));
            EditorGUILayout.LabelField(m_dataDir, EditorStyles.textField);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button(UMConfigInspectorConst.SELECT_DATA_OUT_DIRECTORY))
            {
                m_selectDir =
                    EditorUtility.OpenFolderPanel(UMConfigInspectorConst.DATA_FILES_OUTPUT_DIRECTORY, m_dataDir, "");
                if (m_selectDir != String.Empty && m_selectDir != m_dataDir)
                {
                    m_dataDir = m_selectDir;
                    SaveDirVal(UMConfigInspectorConst.KEY_DATA_DIR, m_dataDir);
                }
            }
        }

        private void DrawConfigScriptsOutputDirectory()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(UMConfigInspectorConst.CONFIG_SCRIPTS_OUTPUT_DIRECTORY, GUILayout.Width(150));
            EditorGUILayout.LabelField(m_scriptsDir, EditorStyles.textField);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button(UMConfigInspectorConst.SELECT_SCRIPTS_OUT_DIRECTORY))
            {
                m_selectDir =
                    EditorUtility.OpenFolderPanel(UMConfigInspectorConst.CONFIG_SCRIPTS_OUTPUT_DIRECTORY, m_scriptsDir,
                        "");
                if (m_selectDir != String.Empty && m_selectDir != m_scriptsDir)
                {
                    m_scriptsDir = m_selectDir;
                    SaveDirVal(UMConfigInspectorConst.KEY_SCRIPTS_DIR, m_scriptsDir);
                }
            }
        }

        private void DrawUpdateConfig()
        {
            if (GUILayout.Button(UMConfigInspectorConst.UPDATE_CONFIG))
            {
                // UMConfigHandler.Create();
            }
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