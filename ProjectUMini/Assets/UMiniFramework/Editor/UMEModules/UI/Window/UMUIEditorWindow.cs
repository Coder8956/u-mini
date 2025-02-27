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
using UnityEngine.UI;

namespace UMiniFramework.Editor.UMEModules.UI
{
    public class UMUIEditorWindow : EditorWindow
    {
        private const string GUI_STYLE_HELPBOX = "HelpBox";
        private const string PREFAB_EXTENSION = ".prefab";
        private const string RESOURCES_FOLDER_NAME = "Resources";

        private int selectedTabIndex = 0; // 当前选中的 tab 的索引

        private string[] tabNames = {"Create Panel", "Create Dialog", "Common"}; // 导航条的选项

        private GUIStyle m_redLabelStyle;

        #region CreatePanel Field

        private bool m_isAddImageComponent = true;

        /// <summary>
        /// 存放所有继承 UMUIPanel 的子类
        /// </summary>
        private List<Type> m_allUITypes = null;

        private Dictionary<string, Type> m_allUITypesDic = new Dictionary<string, Type>();

        // UI类型-下拉框选项
        private const string INVALID_UMUI = "INVALID UMUI";
        private string[] uiClass_options = null;
        private int uiClass_selectedIndex = 0; // 默认选中的项

        // 记录 UI Prefab 文件夹根路径
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

            int oldIndex = uiClass_selectedIndex;
            // 使用 EditorGUILayout.Popup 创建下拉选择框
            // 不使用有Label的Popup uiClass_selectedIndex = EditorGUILayout.Popup("Select UI Panel", uiClass_selectedIndex, uiClass_options, GUILayout.Width(500));
            uiClass_selectedIndex = EditorGUILayout.Popup(uiClass_selectedIndex, uiClass_options, GUILayout.Width(200));
            // UMUtilDebug.Log($"Update UI Class selected index:{uiClass_selectedIndex}");

            // 判断一下当选择的 UI 更新了,再更新相关数据
            if (oldIndex != uiClass_selectedIndex)
            {
                // UMUtilDebug.Log($"Curt UI Class Option: {CurtUIClassOption()}");

                // 清除路径
                m_panelPrefabRootFolder = string.Empty;

                if (CurtUIClassOption() == INVALID_UMUI)
                {
                    m_createPanelType = null;
                    m_createPanelConfig = null;
                }
                else
                {
                    m_createPanelType = m_allUITypesDic[uiClass_options[uiClass_selectedIndex]];
                    m_createPanelConfig =
                        (UMUIPanelConfig) Attribute.GetCustomAttribute(m_createPanelType, typeof(UMUIPanelConfig));
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
            if (CurtUIClassOption() == INVALID_UMUI) return;
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
                        m_panelPrefabRootFolder = selectedPath;
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

            // GUI.enabled = m_panelPrefabRootFolder != string.Empty;
        }

        /// <summary>
        /// 绘制创建 UI Panel 的路径Label
        /// </summary>
        private void DrawUIPanelCreatePath()
        {
            m_panelPrefabFullPath = Path.Combine(m_panelPrefabRootFolder, m_createPanelConfig.LoadPath);
            m_panelPrefabFullPath = UMEUtilCommon.FormatPathSeparator(m_panelPrefabFullPath);
            m_panelPrefabFullPath = string.Concat(m_panelPrefabFullPath, PREFAB_EXTENSION);

            m_panelPrefabAssetDataPath = UMEUtilCommon.GetAssetDataPath(m_panelPrefabFullPath);

            GUILayout.Label($"Prefab AssetData Path: {m_panelPrefabAssetDataPath}", GUI_STYLE_HELPBOX);
            GUILayout.Label($"Prefab Full Path: {m_panelPrefabFullPath}", GUI_STYLE_HELPBOX);

            if (m_createPanelConfig.LoadType == UMResLoadType.Resources
                && !m_panelPrefabAssetDataPath.Contains(RESOURCES_FOLDER_NAME))
            {
                GUILayout.Label(
                    "This UIPanel is loaded as Resources, but the AssetData path does not contain the Resources folder.",
                    m_redLabelStyle);
            }
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
        /// 绘制 UIPanel 信息
        /// </summary>
        private void DrawUIPanelInfo()
        {
            if (CurtUIClassOption() == INVALID_UMUI)
            {
                GUILayout.Label("Please select a valid UI Class.", m_redLabelStyle);
            }
            else
            {
                GUILayout.Label($"Load Type: {m_createPanelConfig.LoadType.ToString()}", GUI_STYLE_HELPBOX);
                GUILayout.Label($"Load Path: {m_createPanelConfig.LoadPath}", GUI_STYLE_HELPBOX);
            }
        }

        /// <summary>
        /// 绘制创建 UIPanel 的选项
        /// </summary>
        private void DrawUIPanelCreateOptions()
        {
            // 绘制一个 bool 选项，控制是否添加 Image 组件
            m_isAddImageComponent = EditorGUILayout.Toggle("Add Image Component", m_isAddImageComponent);
        }

        /// <summary>
        /// 绘制创建 UIPanel 按钮 
        /// </summary>
        private void DrawCreateUIPanelBtn()
        {
            if (GUILayout.Button("Create Panel Prefab"))
            {
                CreateUIPanelPrefab();
            }
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
                List<Type> components = new List<Type>();

                components.Add(typeof(RectTransform));
                components.Add(typeof(CanvasRenderer));

                if (m_isAddImageComponent)
                {
                    components.Add(typeof(Image));
                }

                components.Add(m_allUITypesDic[CurtUIClassOption()]);


                GameObject createPanel =
                    new GameObject(m_allUITypesDic[CurtUIClassOption()].Name, components.ToArray());

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
                EditorUtility.DisplayDialog("Tip", $"{CurtUIClassOption()} Prefab is created.", "OK");

                // 找到你想高亮的 UI 资源
                string uiPrefabPath = m_panelPrefabAssetDataPath; // 资源的路径
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(uiPrefabPath);
                if (asset != null)
                {
                    // 高亮这个资源
                    EditorGUIUtility.PingObject(asset);
                }
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
            if (m_redLabelStyle == null)
            {
                // 创建一个新的 GUIStyle
                m_redLabelStyle = new GUIStyle(GUI.skin.label);
                // 设置字体颜色为红色
                m_redLabelStyle.normal.textColor = Color.red;
            }

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
            DrawPopupUIPanel();
            DrawUIPanelInfo();
            DrawSelectUIPanelPrefabRootFolder();

            if (CurtUIClassOption() != INVALID_UMUI
                && (m_panelPrefabRootFolder != String.Empty))
            {
                DrawUIPanelCreatePath();
                DrawUIPanelCreateOptions();
                DrawCreateUIPanelBtn();
            }
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