using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.UI
{
    public class UMUIEditorWindow : EditorWindow
    {
        private const string GUI_STYLE_HELPBOX = "HelpBox";

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
                    m_panelPrefabRootFolder = selectedPath;
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
            GUILayout.Label($"Prefab Path Type: {m_createPanelConfig.PathType.ToString()}", GUI_STYLE_HELPBOX);
            GUILayout.Label($"Prefab Load Path: {m_createPanelConfig.Path}", GUI_STYLE_HELPBOX);

            string prefabCreatePath = string.Empty;
            if (m_createPanelConfig.PathType == UMResPathType.Resources)
            {
                prefabCreatePath = $"{m_panelPrefabRootFolder}/{m_createPanelConfig.Path}";
            }
            else
            {
                prefabCreatePath = "invalid path";
            }

            GUILayout.Label($"Prefab Create Path: {prefabCreatePath}", GUI_STYLE_HELPBOX);
            
            // TODO:检测文件是否存在
        }

        /// <summary>
        /// 绘制创建 UIPanel 按钮 
        /// </summary>
        private void DrawCreateUIPanelBtn()
        {
            bool enable = CurtUIClassOption() != INVALID_UMUI;
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