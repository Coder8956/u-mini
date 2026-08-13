using System;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor
{
    [CustomEditor(typeof(UMOConfig))]
    public class UMOConfigInspe : UnityEditor.Editor
    {
        // ==================== 私有字段（运行时状态） ====================

        private bool m_foConfigTables = true; // 控制折叠状态
        private static readonly FieldInfo Field_UMConfig_TableDic =
            typeof(UMOConfig).GetField("m_tableDic", BindingFlags.NonPublic | BindingFlags.Instance);
        private Dictionary<Type, UMConfigTableBase> m_tableDic;

        // ==================== 生命周期 ====================

        private void OnEnable()
        {
            m_tableDic = Field_UMConfig_TableDic != null
                ? (Dictionary<Type, UMConfigTableBase>)Field_UMConfig_TableDic.GetValue(target)
                : null;
        }

        // ==================== 公开接口 ====================

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (m_tableDic == null)
            {
                EditorGUILayout.HelpBox("Failed to retrieve m_tableDic via reflection.", MessageType.Warning);
                return;
            }

            // 统计非语言配置表数量
            int tableCount = 0;
            foreach (var v in m_tableDic.Values)
            {
                if (!(v is IUMLangTable))
                    tableCount++;
            }

            m_foConfigTables = EditorGUILayout.Foldout(m_foConfigTables, $"Config Tables ({tableCount})");
            if (m_foConfigTables)
            {
                // 绘制 Config Table
                EditorGUI.indentLevel++; // 增加缩进
                int tableIndex = 0;
                foreach (var kv in m_tableDic)
                {
                    // 跳过多语言配置表，由 UMLocalCfg Inspector 绘制
                    if (kv.Value is IUMLangTable)
                        continue;

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

                    if (GUILayout.Button("Ping Table Asset Object"))
                    {
                        string textAssetPath = string.Concat(kv.Value.AssetPath, ".json");
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
                        string textAssetPath = string.Concat(kv.Value.AssetPath, ".json");
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
