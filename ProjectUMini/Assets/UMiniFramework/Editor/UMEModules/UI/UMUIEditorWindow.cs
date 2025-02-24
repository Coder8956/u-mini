using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.UI
{
    public class UMUIEditorWindow : EditorWindow
    {
        private const string GUI_STYLE_HELPBOX = "HelpBox";
        private const string PREFAB_EXTENSION = ".prefab";

        private int selectedTabIndex = 0; // 当前选中的 tab 的索引

        private string[] tabNames = {"Create Panel", "Create Dialog", "Common"}; // 导航条的选项

        #region CreatePanel Field

        /// <summary>
        /// 存放所有继承 UMUIPanel 的子类
        /// </summary>
        private List<Type> m_allUITypes = null;

        private Dictionary<string, Type> m_allUITypesDic = new Dictionary<string, Type>();

        // UI类型-下拉框选项
        private const string INVALID_UMUI = "INVALID UMUI";
        private string[] uiClass_options = null;
        private int uiClass_selectedIndex = 0; // 默认选中的项

        // 记录 PanelPrefab 根文件夹路径
        private string m_panelPrefabRootFolder = string.Empty;

        // 记录 PanelPrefab 完整路径
        private string m_panelPrefabFullPath = string.Empty;

        // 记录 PanelPrefab AssetData 路径
        private string m_panelPrefabAssetDataPath = string.Empty;


        // 需要创建的 UIPanel Type
        private Type m_createPanelType;
        private UMUIPanelConfig m_createPanelConfig;

        /// <summary>
        /// 查询所有继承 UMUIPanel 的类
        /// </summary>
        private void QueryAllUIPanelClasses()
        {
            m_allUITypesDic.Clear();

            m_allUITypesDic.Add(INVALID_UMUI, null);

            // 获取当前程序集中的所有类型
            var types = Assembly.GetAssembly(typeof(UMUIPanel)).GetTypes();

            // 获取继承自BaseClass的所有子类
            m_allUITypes = types.Where(t => t.IsSubclassOf(typeof(UMUIPanel)) && !t.IsAbstract).ToList();

            // 判断是否有有效的 UI 类型
            if (m_allUITypes == null || m_allUITypes.Count < 1)
            {
                return;
            }

            // 输出所有继承自BaseClass的子类名称
            foreach (var type in m_allUITypes)
            {
                m_allUITypesDic.Add(type.Name, type);
            }

            uiClass_options = m_allUITypesDic.Keys.ToArray();
        }

        /// <summary>
        /// 绘制 UIPanel 选择框
        /// </summary>
        private void DrawPopupUIPanel()
        {
            EditorGUILayout.BeginHorizontal();

            // 在窗口中绘制一个标签
            // GUILayout.Label("Select UI Panel", EditorStyles.boldLabel);
            GUILayout.Label("Select UIPanel", GUILayout.Width(90)); // 设置Label的宽度

            // 使用 EditorGUILayout.Popup 创建下拉选择框
            // 不使用有Label的Popup uiClass_selectedIndex = EditorGUILayout.Popup("Select UI Panel", uiClass_selectedIndex, uiClass_options, GUILayout.Width(500));
            int newIndex = EditorGUILayout.Popup(uiClass_selectedIndex, uiClass_options, GUILayout.Width(200));

            if (uiClass_selectedIndex != newIndex)
            {
                uiClass_selectedIndex = newIndex;
                // UMUtilDebug.Log($"Update UI Class selected index:{uiClass_selectedIndex}");
                if (CurtUIClassOption() != INVALID_UMUI)
                {
                    m_createPanelType = m_allUITypesDic[uiClass_options[uiClass_selectedIndex]];
                    m_createPanelConfig =
                        (UMUIPanelConfig) Attribute.GetCustomAttribute(m_createPanelType, typeof(UMUIPanelConfig));

                    // 更新 UI 路径
                    string loadPathWithExtension = string.Concat(m_createPanelConfig.LoadPath, PREFAB_EXTENSION);
                    m_panelPrefabFullPath = Path.Combine(m_panelPrefabRootFolder, loadPathWithExtension);
                    m_panelPrefabFullPath = UMEUtilCommon.FormatPathSeparator(m_panelPrefabFullPath);

                    m_panelPrefabAssetDataPath = UMEUtilCommon.GetAssetDataPath(m_panelPrefabFullPath);
                    m_panelPrefabAssetDataPath = UMEUtilCommon.FormatPathSeparator(m_panelPrefabAssetDataPath);
                }
            }

            EditorGUILayout.EndHorizontal();

            // 显示当前选择的项
            // GUILayout.Label("You selected: " + uiClass_options[uiClass_selectedIndex]);
        }

        /// <summary>
        /// 绘制选择 UIPanel 预制体根文件夹
        /// </summary>
        private void DrawSelectUIPanelPrefabRootFolder()
        {
            EditorGUILayout.BeginHorizontal();

            int layoutHeight = 20;

            // GUILayout.Label("Select UIPanel Prefab Folder:", EditorStyles.boldLabel);
            GUILayout.Label("UI Prefab Root Folder", GUILayout.Width(120), GUILayout.Height(layoutHeight));

            if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 打开文件夹选择框
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (UMEUtilCommon.IsContainsDataPath(selectedPath))
                    {
                        m_panelPrefabRootFolder = selectedPath;
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Tip",
                            "Only the path under the current project Assets folder can be selected.",
                            "OK");
                    }
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(layoutHeight)))
            {
                // 清除路径
                m_panelPrefabRootFolder = string.Empty;
            }

            // GUILayout.Label("Selected Folder Path: ", GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));
            GUILayout.Label(m_panelPrefabRootFolder, GUI_STYLE_HELPBOX, GUILayout.Height(layoutHeight));

            EditorGUILayout.EndHorizontal();

            GUI.enabled = m_panelPrefabRootFolder != string.Empty;
        }

        /// <summary>
        /// 当前UI类型选项
        /// </summary>
        /// <returns></returns>
        private string CurtUIClassOption()
        {
            return uiClass_options[uiClass_selectedIndex];
        }

        /// <summary>
        /// 绘制创建 UIPanel 信息
        /// </summary>
        private void DrawUIPanelInfo()
        {
            if (CurtUIClassOption() == INVALID_UMUI) return;
            if (m_createPanelConfig.LoadType != UMResLoadType.Resources)
            {
                // 创建一个新的 GUIStyle
                GUIStyle redLabelStyle = new GUIStyle(GUI.skin.label);
                // 设置字体颜色为红色
                redLabelStyle.normal.textColor = Color.red;

                GUILayout.Label($"Invalid Path Type:{m_createPanelConfig.LoadType}", redLabelStyle);
                return;
            }

            GUILayout.Label($"Prefab Load Type: {m_createPanelConfig.LoadType.ToString()}", GUI_STYLE_HELPBOX);
            GUILayout.Label($"Prefab Load Path: {m_createPanelConfig.LoadPath}", GUI_STYLE_HELPBOX);
            GUILayout.Label($"Prefab AssetData Path: {m_panelPrefabAssetDataPath}", GUI_STYLE_HELPBOX);
            GUILayout.Label($"Prefab Full Path: {m_panelPrefabFullPath}", GUI_STYLE_HELPBOX);
        }

        /// <summary>
        /// 绘制创建 UIPanel 按钮 
        /// </summary>
        private void DrawCreateUIPanelBtn()
        {
            bool enable = CurtUIClassOption() != INVALID_UMUI
                          && m_panelPrefabRootFolder != string.Empty;
            GUI.enabled = enable;
            if (GUILayout.Button("Create Panel Prefab"))
            {
                CreateUIPanelPrefab();
            }

            GUI.enabled = true;
        }

        /// <summary>
        /// 创建 UIPanel Prefab
        /// </summary>
        private void CreateUIPanelPrefab()
        {
            bool exists = UMEUtilCommon.CheckPrefabExists(m_panelPrefabAssetDataPath);
            if (exists)
            {
                EditorUtility.DisplayDialog("Tip",
                    $"Cannot create. Because the {m_panelPrefabAssetDataPath}.prefab already exists.",
                    "OK");
            }
            else
            {
                GameObject createPanel =
                    new GameObject(m_allUITypesDic[CurtUIClassOption()].Name,
                        typeof(RectTransform),
                        typeof(CanvasRenderer),
                        m_allUITypesDic[CurtUIClassOption()]);

                RectTransform crt = createPanel.GetComponent<RectTransform>();

                // 修改锚点
                crt.anchorMin = Vector2.zero;
                crt.anchorMax = Vector2.one;

                // 修改边界偏移量
                crt.offsetMin = Vector2.zero;
                crt.offsetMax = Vector2.zero;

                // 判断存放预制体的文件夹是否存在
                string panelFolder = Path.GetDirectoryName(m_panelPrefabFullPath);
                if (!Directory.Exists(panelFolder))
                {
                    Directory.CreateDirectory(panelFolder);
                }
                
                PrefabUtility.SaveAsPrefabAsset(createPanel, m_panelPrefabAssetDataPath);
                DestroyImmediate(createPanel);
                AssetDatabase.Refresh();
            }
        }

        #endregion

        [MenuItem("UMUtils/UI/UMUI-Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<UMUIEditorWindow>();
            window.titleContent = new GUIContent("UMUI Editor");
            window.Show();
        }

        private void OnEnable()
        {
            QueryAllUIPanelClasses();
        }

        private void OnGUI()
        {
            // 创建一个水平布局的导航条
            EditorGUILayout.BeginHorizontal("box");

            // 在开始时添加弹性空间，使内容居中
            GUILayout.FlexibleSpace();

            for (int i = 0; i < tabNames.Length; i++)
            {
                // 使用按钮作为导航条项，每次点击按钮时切换选中的 tab
                GUI.backgroundColor = selectedTabIndex == i ? Color.cyan : Color.gray; // 当前选中的 tab 高亮
                if (GUILayout.Button(tabNames[i], GUILayout.Width(100)))
                {
                    selectedTabIndex = i; // 更新选中的 tab 索引
                }
            }

            // 在按钮后添加弹性空间，确保按钮居中
            GUILayout.FlexibleSpace();

            // 结束水平布局
            EditorGUILayout.EndHorizontal();

            // 根据选中的 tab 显示不同的内容
            switch (selectedTabIndex)
            {
                case 0:
                    ShowCreatePanelContent();
                    break;
                case 1:
                    ShowCreateDialogContent();
                    break;
                case 2:
                    ShowCommonContent();
                    break;
            }
        }

        /// <summary>
        /// Create Panel 的内容
        /// </summary>
        private void ShowCreatePanelContent()
        {
            // EditorGUILayout.HelpBox("This is the content of Tab 1.", MessageType.Info);
            DrawSelectUIPanelPrefabRootFolder();
            DrawPopupUIPanel();
            DrawUIPanelInfo();
            DrawCreateUIPanelBtn();
        }

        /// <summary>
        /// Create Dialog 的内容
        /// </summary>
        private void ShowCreateDialogContent()
        {
            EditorGUILayout.HelpBox("This is the content of Tab 2.", MessageType.Info);
        }

        // Common 的内容
        private void ShowCommonContent()
        {
            EditorGUILayout.HelpBox("This is the content of Tab 3.", MessageType.Info);
        }
    }
}