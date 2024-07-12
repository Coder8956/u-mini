using UMiniFramework.Editor.Const;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMWindows.ConfigWindow
{
    public class UMConfigWindow : EditorWindow
    {
        /// <summary>
        /// Excel 配置目录 文本常量
        /// </summary>
        private const string EXCEL_CONFIG_DIRECTORY = "Excel Config Directory: ";

        /// <summary>
        /// 选择目录 文本常量
        /// </summary>
        private const string SELECT_DIRECTORY = "Select Directory";

        /// <summary>
        /// Json文件输出目录 文本常量
        /// </summary>
        private const string JSON_FILES_OUTPUT_DIRECTORY = "Json Files Output: ";

        /// <summary>
        /// 配置脚本文件输出目录 文本常量
        /// </summary>
        private const string CONFIG_SCRIPTS_OUTPUT_DIRECTORY = "Config Scripts Output: ";

        /// <summary>
        /// 更新配置 文本常量
        /// </summary>
        private const string UPDATE_CONFIG = "Update Config";

        [MenuItem(UMEditorConst.UMEDITOR_WINDOWS_TITLE_ROOT + "/Config")]
        private static void ShowWindow()
        {
            var window = GetWindow<UMConfigWindow>();
            window.titleContent = new GUIContent("UMConfigWindow");
            window.Show();
            window.maxSize = new Vector2(window.maxSize.x, 100);
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
            EditorGUILayout.LabelField(EXCEL_CONFIG_DIRECTORY, EditorStyles.textField, GUILayout.Width(150));
            EditorGUILayout.LabelField("", EditorStyles.textField);
            if (GUILayout.Button(SELECT_DIRECTORY, GUILayout.Width(140)))
            {
                EditorUtility.OpenFolderPanel(EXCEL_CONFIG_DIRECTORY, "", "");
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawJsonFilesOutputDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(JSON_FILES_OUTPUT_DIRECTORY, EditorStyles.textField, GUILayout.Width(150));
            EditorGUILayout.LabelField("", EditorStyles.textField);
            if (GUILayout.Button(SELECT_DIRECTORY, GUILayout.Width(140)))
            {
                EditorUtility.OpenFolderPanel(JSON_FILES_OUTPUT_DIRECTORY, "", "");
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawConfigScriptsOutputDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(CONFIG_SCRIPTS_OUTPUT_DIRECTORY, EditorStyles.textField, GUILayout.Width(150));
            EditorGUILayout.LabelField("", EditorStyles.textField);
            if (GUILayout.Button(SELECT_DIRECTORY, GUILayout.Width(140)))
            {
                EditorUtility.OpenFolderPanel(CONFIG_SCRIPTS_OUTPUT_DIRECTORY, "", "");
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawUpdateConfig()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(UPDATE_CONFIG))
            {
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}