using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor
{
    public class UMConfigWindow : EditorWindow
    {
        private const string KEY_CONFIG_PATH = "UMCFG_CONFIG_PATH";
        private const string KEY_SCRIPTS_PATH = "UMCFG_SCRIPTS_PATH";
        private const string KEY_DATA_PATH = "UMCFG_DATA_PATH";

        private const string READ_FILES_TIP_1 = "Please click read button";
        private const string READ_FILES_TIP_2 = "The configuration file was not read";
        private const string READ_FILES_TIP_3 = "{0} configuration files were read";

        private const string GUI_STYLE_HELPBOX = "HelpBox";

        private const int LINE_HEIGHT = 20;
        private const int MAX_SCROLL_HEIGHT = 400;


        private string m_configInputDir;
        private string m_scriptOutputDir;
        private string m_jsonOutputDir;

        private string m_readFilesTip;

        private GUIStyle m_redLabelStyle;

        private Vector2 m_scrollPosition;
        private float m_scrollViewHeight;

        private readonly List<string> m_configFiles = new();
        
        [MenuItem("UMini/Window/Update Config")]
        private static void ShowWindow()
        {
            UMConfigWindow window = GetWindow<UMConfigWindow>();
            window.titleContent = new GUIContent("UMConfig Editor");
            window.Show();
        }
        
        private void OnEnable()
        {
            ReadPaths();
            m_readFilesTip = READ_FILES_TIP_1;
        }

        private void OnGUI()
        {
            InitStyles();

            DrawFolderPath(
                "Config Input Directory",
                ref m_configInputDir,
                KEY_CONFIG_PATH,
                "Select Config Input Folder",
                true);
            
            DrawReadConfigFiles();
            
            GUI.enabled = m_configFiles.Count > 0;

            try
            {
                DrawFolderPath(
                    "Config Script Output Directory",
                    ref m_scriptOutputDir,
                    KEY_SCRIPTS_PATH,
                    "Select Script Output Folder");


                DrawFolderPath(
                    "Config Json Output Directory",
                    ref m_jsonOutputDir,
                    KEY_DATA_PATH,
                    "Select Json Output Folder");


                DrawOutputDirWarning();

                DrawUpdateButtons();
            }
            finally
            {
                GUI.enabled = true;
            }
        }
        
        private void DrawFolderPath(
            string title,
            ref string path,
            string prefsKey,
            string dialogTitle,
            bool showOpen = false)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(title, GUILayout.Width(180), GUILayout.Height(LINE_HEIGHT));

            if (GUILayout.Button("Select",
                    GUILayout.Width(50),
                    GUILayout.Height(LINE_HEIGHT)))
            {
                string result =
                    EditorUtility.OpenFolderPanel(
                        dialogTitle,
                        Application.dataPath,
                        "");


                if (!string.IsNullOrEmpty(result))
                {
                    path = result;
                    EditorPrefs.SetString(prefsKey, path);

                    if (prefsKey == KEY_CONFIG_PATH)
                    {
                        m_readFilesTip = READ_FILES_TIP_1;
                        m_configFiles.Clear();
                    }
                }
            }

            if (GUILayout.Button("Clear",
                    GUILayout.Width(50),
                    GUILayout.Height(LINE_HEIGHT)))
            {
                path = string.Empty;
                EditorPrefs.SetString(prefsKey, path);

                if (prefsKey == KEY_CONFIG_PATH)
                {
                    m_configFiles.Clear();
                }
            }

            if (showOpen &&
                GUILayout.Button("Open",
                    GUILayout.Width(50),
                    GUILayout.Height(LINE_HEIGHT)))
            {
                OpenFolder(path);
            }

            GUILayout.Label(
                path,
                GUI_STYLE_HELPBOX,
                GUILayout.Height(LINE_HEIGHT));

            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawReadConfigFiles()
        {
            if (string.IsNullOrEmpty(m_configInputDir))
                return;
            
            if (GUILayout.Button("Read Config Files"))
            {
                m_configFiles.Clear();

                UMConfigHandler.GetAllExcelFiles(
                    m_configInputDir,
                    m_configFiles);
                
                if (m_configFiles.Count == 0)
                {
                    m_readFilesTip = READ_FILES_TIP_2;
                }
                else
                {
                    m_readFilesTip =
                        string.Format(
                            READ_FILES_TIP_3,
                            m_configFiles.Count);
                }

                m_scrollViewHeight =
                    Mathf.Clamp(
                        m_configFiles.Count * LINE_HEIGHT,
                        0,
                        MAX_SCROLL_HEIGHT);
            }
            
            GUILayout.Label(
                m_readFilesTip,
                GUI_STYLE_HELPBOX,
                GUILayout.Height(LINE_HEIGHT));
            
            if (m_configFiles.Count == 0)
                return;
            
            m_scrollPosition =
                GUILayout.BeginScrollView(
                    m_scrollPosition,
                    GUILayout.Height(m_scrollViewHeight));

            for (int i = 0; i < m_configFiles.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(
                    $"Config-{i:D5}",
                    GUILayout.Width(100));


                GUILayout.Label(m_configFiles[i]);

                EditorGUILayout.EndHorizontal();
            }
            
            GUILayout.EndScrollView();
        }
        
        private void DrawOutputDirWarning()
        {
            GUILayout.Label(
                "The output directory is cleared when an update is performed.",
                m_redLabelStyle);
        }

        private void DrawUpdateButtons()
        {
            if (string.IsNullOrEmpty(m_jsonOutputDir) ||
                string.IsNullOrEmpty(m_scriptOutputDir))
                return;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Update Data"))
            {
                UMConfigHandler.UpdateConfig(
                    m_configFiles,
                    m_scriptOutputDir,
                    m_jsonOutputDir,
                    UMConfigUpdateMode.Data);
            }

            if (GUILayout.Button("Update Scripts"))
            {
                UMConfigHandler.UpdateConfig(
                    m_configFiles,
                    m_scriptOutputDir,
                    m_jsonOutputDir,
                    UMConfigUpdateMode.Scripts);
            }

            if (GUILayout.Button("Update Data&Scripts"))
            {
                UMConfigHandler.UpdateConfig(
                    m_configFiles,
                    m_scriptOutputDir,
                    m_jsonOutputDir,
                    UMConfigUpdateMode.DataAndScripts);
            }

            EditorGUILayout.EndHorizontal();
        }
        
        private void InitStyles()
        {
            if (m_redLabelStyle != null)
                return;


            m_redLabelStyle =
                new GUIStyle(GUI.skin.label)
                {
                    normal =
                    {
                        textColor = Color.red
                    }
                };
        }

        private void OpenFolder(string path)
        {
            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Invalid Config Directory",
                    "Please select a valid directory.",
                    "OK");
            }
        }

        private void ReadPaths()
        {
            m_configInputDir =
                EditorPrefs.GetString(KEY_CONFIG_PATH, "");


            m_scriptOutputDir =
                EditorPrefs.GetString(KEY_SCRIPTS_PATH, "");


            m_jsonOutputDir =
                EditorPrefs.GetString(KEY_DATA_PATH, "");
        }
    }
}