using System;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Editor
{
    public class UMUIWindow : EditorWindow
    {
        private const string SavePathKey = "UMUI_Save_Path";

        private List<Type> m_panelTypes = new();
        private string[] m_panelNames;

        private int m_selectIndex;

        private DefaultAsset m_saveFolder;
        private string m_savePath;

        private bool m_pathValid;

        private bool m_prefabExist;
        private string m_prefabPath;

        private bool m_addImageComponent = true;
        private bool m_addCloseButton = true;
        private bool m_addSafeArea = true;

        [MenuItem("UMini/Window/Create UI Prefab")]
        private static void ShowWindow()
        {
            var window = GetWindow<UMUIWindow>();

            window.titleContent =
                new GUIContent("Create UMUI Panel");

            window.minSize =
                new Vector2(650, 200);

            window.Show();
        }

        private void OnEnable()
        {
            RefreshPanelTypes();

            m_savePath =
                EditorPrefs.GetString(
                    SavePathKey,
                    "Assets");

            m_saveFolder =
                AssetDatabase.LoadAssetAtPath<DefaultAsset>(
                    m_savePath);

            CheckPath();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);


            DrawPanelSelect();


            GUILayout.Space(10);


            DrawFolderSelect();


            GUILayout.Space(10);


            DrawCreateButton();
        }

        private void DrawPanelSelect()
        {
            EditorGUILayout.LabelField(
                "UI Panel Class",
                EditorStyles.boldLabel);


            if (m_panelNames.Length == 0)
            {
                EditorGUILayout.HelpBox(
                    "没有找到继承 UMUIPanelBase 的类",
                    MessageType.Warning);

                return;
            }


            m_selectIndex =
                EditorGUILayout.Popup(
                    "Panel",
                    m_selectIndex,
                    m_panelNames);
        }

        private void DrawFolderSelect()
        {
            EditorGUILayout.LabelField(
                "Prefab Save Folder",
                EditorStyles.boldLabel);


            var folder =
                EditorGUILayout.ObjectField(
                        "Folder",
                        m_saveFolder,
                        typeof(DefaultAsset),
                        false)
                    as DefaultAsset;


            if (folder != m_saveFolder)
            {
                m_saveFolder = folder;

                m_savePath =
                    AssetDatabase.GetAssetPath(folder);


                EditorPrefs.SetString(
                    SavePathKey,
                    m_savePath);


                CheckPath();
            }


            if (!m_pathValid)
            {
                GUI.color = Color.red;

                EditorGUILayout.LabelField(
                    "目录不可用，请选择 Assets 下有效目录");

                GUI.color = Color.white;
            }
            else
            {
                EditorGUILayout.LabelField(
                    "Path : " + m_savePath);
            }
        }

        private void DrawCreateButton()
        {
            CheckPrefabExist();

            if (m_prefabExist)
            {
                EditorGUILayout.HelpBox(
                    $"Prefab 已存在:\n{m_prefabPath}",
                    MessageType.Info);


                if (GUILayout.Button("Ping Existing Prefab"))
                {
                    var prefab =
                        AssetDatabase.LoadAssetAtPath<GameObject>(
                            m_prefabPath);


                    if (prefab != null)
                    {
                        EditorGUIUtility.PingObject(prefab);
                    }
                }
            }
            else
            {
                // 创建选项
                EditorGUILayout.Space(5);

                m_addImageComponent =
                    EditorGUILayout.Toggle(
                        "Add Image Component",
                        m_addImageComponent);

                m_addCloseButton =
                    EditorGUILayout.Toggle(
                        "Add Close Button",
                        m_addCloseButton);
                
                m_addSafeArea =
                    EditorGUILayout.Toggle(
                        "Add Safe Area",
                        m_addCloseButton);
            }

            GUI.enabled =
                m_pathValid &&
                m_panelTypes.Count > 0 &&
                !m_prefabExist;

            if (GUILayout.Button(
                "Create UI Prefab",
                GUILayout.Height(35)))
            {
                CreateUIPrefab();
            }

            GUI.enabled = true;
        }

        private void CreateUIPrefab()
        {
            Type panelType =
                m_panelTypes[m_selectIndex];

            string panelName =
                panelType.Name;

            string prefabPath =
                $"{m_savePath}/{panelName}.prefab";

            if (AssetDatabase.LoadAssetAtPath<GameObject>(
                prefabPath))
            {
                EditorUtility.DisplayDialog(
                    "Create Failed",
                    $"Prefab 已存在:\n{prefabPath}",
                    "OK");

                return;
            }

            GameObject go =
                new GameObject(panelName);

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            go.AddComponent<CanvasRenderer>();

            // 添加ImageGroup组件
            if (m_addImageComponent)
            {
                go.AddComponent<Image>();
            }

            go.AddComponent(panelType);

            if (m_addCloseButton)
            {
                GameObject btnGo = new GameObject("BtnClose");
                btnGo.transform.SetParent(go.transform, false);

                RectTransform btnRect = btnGo.AddComponent<RectTransform>();
                btnRect.anchorMin = new Vector2(1, 1);
                btnRect.anchorMax = new Vector2(1, 1);
                btnRect.pivot = new Vector2(0.5f, 0.5f);
                btnRect.sizeDelta = new Vector2(200, 200);
                btnRect.anchoredPosition = new Vector2(-110, -110);

                btnGo.AddComponent<CanvasRenderer>();
                Image btnImg = btnGo.AddComponent<Image>();
                btnImg.color = new Color(0.8f, 0.2f, 0.2f, 1f);

                btnGo.AddComponent<Button>();
            }
            
            if (m_addSafeArea)
            {
                GameObject safeArea = new GameObject("SafeArea");
                safeArea.transform.SetParent(go.transform, false);

                RectTransform safeAreaRect = safeArea.AddComponent<RectTransform>();
                UMUIUtils.StretchFull(safeAreaRect);
                safeArea.AddComponent<UMUISafeArea>();
            }

            PrefabUtility.SaveAsPrefabAsset(
                go,
                prefabPath);

            DestroyImmediate(go);

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Success",
                $"创建成功:\n{prefabPath}",
                "OK");
        }

        private void RefreshPanelTypes()
        {
            m_panelTypes.Clear();


            var baseType =
                typeof(
                    UMUIPanelBase);


            foreach (var assembly in
                AppDomain.CurrentDomain
                    .GetAssemblies())
            {
                try
                {
                    var types =
                        assembly.GetTypes();


                    foreach (var type in types)
                    {
                        if (type.IsAbstract)
                            continue;


                        if (baseType.IsAssignableFrom(type))
                        {
                            m_panelTypes.Add(type);
                        }
                    }
                }
                catch
                {
                }
            }


            m_panelNames =
                m_panelTypes
                    .Select(x => x.Name)
                    .ToArray();
        }

        private void CheckPath()
        {
            m_pathValid =
                false;


            if (string.IsNullOrEmpty(m_savePath))
                return;


            if (!m_savePath.StartsWith("Assets"))
                return;


            if (!AssetDatabase.IsValidFolder(
                m_savePath))
                return;


            m_pathValid = true;
        }

        private void CheckPrefabExist()
        {
            m_prefabExist = false;


            if (!m_pathValid)
                return;


            if (m_panelTypes.Count == 0)
                return;


            string panelName =
                m_panelTypes[m_selectIndex].Name;


            m_prefabPath =
                $"{m_savePath}/{panelName}.prefab";


            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    m_prefabPath);


            m_prefabExist =
                prefab != null;
        }
    }
}