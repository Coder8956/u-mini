using System;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules.Config;
using UMiniFramework.Runtime.Modules.Config.Base;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.Config.Inspector
{
    [CustomEditor(typeof(UMConfig))]
    public class UMConfigInspe : UnityEditor.Editor
    {
        private bool m_foConfigTables = true; // 控制折叠状态
        private static FieldInfo Field_UMConfig_TableDic;
        private Dictionary<Type, UMConfigTable> m_tableDic;

        // private List<TextAsset> m_tableAssets;
        private List<string> m_tableAssetPaths;
        private GUIStyle m_redLabelStyle;

        private void OnEnable()
        {
            Field_UMConfig_TableDic = UMEUtilCommon.GetObjectNoPublicField(typeof(UMConfig), "m_tableDic");
            m_tableDic = (Dictionary<Type, UMConfigTable>) Field_UMConfig_TableDic.GetValue(target);

            // m_tableAssets = new List<TextAsset>();
            m_tableAssetPaths = new List<string>();
            foreach (var tableInfo in m_tableDic.Values)
            {
                string assetPath = string.Concat(tableInfo.AssetPath, ".json");
                // Debug.Log(assetPath);
                // m_tableAssets.Add(AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath));
                m_tableAssetPaths.Add(assetPath);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (m_redLabelStyle == null)
            {
                // 创建一个新的 GUIStyle
                m_redLabelStyle = new GUIStyle(GUI.skin.label);
                // 设置字体颜色为红色
                m_redLabelStyle.normal.textColor = Color.red;
            }

            // EditorGUILayout.LabelField("Register Event Tags", EditorStyles.boldLabel);
            m_foConfigTables = EditorGUILayout.Foldout(m_foConfigTables, $"Config Tables ({m_tableDic.Keys.Count})");
            if (m_foConfigTables)
            {
                // 绘制 Config Table
                EditorGUI.indentLevel++; // 增加缩进
                int tableIndex = 0;
                foreach (var kv in m_tableDic)
                {
                    EditorGUILayout.BeginVertical("helpbox");

                    // 绘制索引
                    string indexFormat = string.Format("{0:D4}", tableIndex);
                    EditorGUILayout.LabelField($"Index[{indexFormat}]", EditorStyles.boldLabel);

                    // 绘制 Table Name
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Register Table: ", EditorStyles.boldLabel, GUILayout.Width(110));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(kv.Key.Name);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制 Table LoadPath
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Table LoadPath: ", EditorStyles.boldLabel, GUILayout.Width(120));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(kv.Value.LoadPath);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制 Table AssetPath
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Table AssetPath: ", EditorStyles.boldLabel, GUILayout.Width(120));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(kv.Value.AssetPath);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // // 绘制 Table 对象
                    // EditorGUILayout.BeginHorizontal();
                    // EditorGUILayout.LabelField($"Table TextAsset Object: ", EditorStyles.boldLabel,
                    //     GUILayout.Width(160));
                    // // 禁用编辑
                    // EditorGUI.BeginDisabledGroup(true);
                    // EditorGUILayout.ObjectField(m_tableAssets[tableIndex], typeof(TextAsset));
                    // // 结束禁用组
                    // EditorGUI.EndDisabledGroup();
                    // EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button("Ping Table Asset Object"))
                    {
                        string textAssetPath = m_tableAssetPaths[tableIndex];
                        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(textAssetPath);
                        if (textAsset != null)
                        {
                            // 高亮这个资源
                            EditorGUIUtility.PingObject(textAsset);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Tip",
                                $"Failed to ping table asset object. Asset object path: {textAssetPath}", "OK");
                        }
                    }

                    if (GUILayout.Button("Open Table Asset Object"))
                    {
                        string textAssetPath = m_tableAssetPaths[tableIndex];
                        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(textAssetPath);
                        if (textAsset != null)
                        {
                            // 打开这个资源
                            AssetDatabase.OpenAsset(textAsset);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Tip",
                                $"Failed to open table asset object. Asset object path: {textAssetPath}", "OK");
                        }
                    }

                    EditorGUILayout.EndVertical();
                    tableIndex++;
                }

                EditorGUI.indentLevel--; // 恢复缩进
            }
        }
    }
}