using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor
{
    [CustomEditor(typeof(UMLocalCfg))]
    public class UMLocalCfgInspe : UnityEditor.Editor
    {
        // ==================== 私有字段（运行时状态） ====================

        private bool m_foOptions = true; // 语言选项折叠状态
        private Vector2 m_scrollPos;

        private static readonly FieldInfo Field_UMConfig_TableDic =
            typeof(UMOConfig).GetField("m_tableDic", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo Field_UMLocalCfg_LocalComponents =
            typeof(UMLocalCfg).GetField("m_localComponents", BindingFlags.NonPublic | BindingFlags.Instance);

        private const int ScrollThreshold = 10;

        private GUIStyle m_markerStyle;

        private string m_lastCode;

        // ==================== 生命周期 ====================

        private void OnEnable()
        {
            m_lastCode = ((UMLocalCfg)target)?.CurtCode;
            EditorApplication.update += PollLanguageChanged;
        }

        private void OnDisable()
        {
            EditorApplication.update -= PollLanguageChanged;
        }

        private void PollLanguageChanged()
        {
            var localCfg = (UMLocalCfg)target;
            if (localCfg == null)
                return;

            string code = localCfg.CurtCode;
            if (code != m_lastCode)
            {
                m_lastCode = code;
                Repaint();
            }
        }

        // ==================== 公开接口 ====================

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (m_markerStyle == null)
            {
                m_markerStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleRight,
                    fontStyle = FontStyle.Bold
                };
            }

            UMLocalCfg localCfg = (UMLocalCfg)target;

            // Language Options
            var options = localCfg.GetOptions();
            int optionCount = options != null ? options.Count : 0;

            m_foOptions = EditorGUILayout.Foldout(m_foOptions, $"Language Options ({optionCount})");
            if (m_foOptions && optionCount > 0)
            {
                EditorGUI.indentLevel++;

                if (optionCount > ScrollThreshold)
                {
                    m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.MaxHeight(160));
                }

                UMConfigTableBase baseTable = GetLangTable();
                IUMLangTable langTable = baseTable as IUMLangTable;
                string langDir = GetLangAssetDir(baseTable);
                string currentCode = localCfg.CurtCode;

                for (int i = 0; i < optionCount; i++)
                {
                    var opt = options[i];
                    string fileName = langTable?.GetLanguageFile(i);
                    bool isCurrent = !string.IsNullOrEmpty(currentCode) && currentCode == opt.code;

                    if (isCurrent)
                    {
                        GUI.color = Color.yellow;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Toggle(isCurrent, GUILayout.Width(20));
                    EditorGUILayout.TextField(opt.type);
                    EditorGUILayout.LabelField(opt.code ?? "?", GUILayout.Width(60));
                    EditorGUILayout.LabelField(fileName ?? "?", GUILayout.Width(100));
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Ping", GUILayout.Width(50)))
                    {
                        PingLangAsset(langDir, fileName);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (isCurrent)
                    {
                        GUI.color = Color.white;
                    }
                }

                if (optionCount > ScrollThreshold)
                {
                    EditorGUILayout.EndScrollView();
                }

                EditorGUI.indentLevel--;
            }

            // Registered Components
            int componentCount = 0;
            if (Field_UMLocalCfg_LocalComponents != null)
            {
                var list = Field_UMLocalCfg_LocalComponents.GetValue(localCfg) as IList;
                if (list != null)
                    componentCount = list.Count;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Registered Components:", EditorStyles.boldLabel, GUILayout.Width(130));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField(componentCount);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        // ==================== 逻辑 ====================

        private void PingLangAsset(string langDir, string fileName)
        {
            if (string.IsNullOrEmpty(langDir) || string.IsNullOrEmpty(fileName))
            {
                Debug.LogWarning("[UMLocalCfgInspe] Failed to resolve language asset path.");
                return;
            }

            string filePath = $"{langDir}/{fileName}";
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
            if (asset != null)
            {
                EditorGUIUtility.PingObject(asset);
            }
            else
            {
                Debug.LogWarning($"[UMLocalCfgInspe] Language asset not found: {filePath}");
            }
        }

        private UMConfigTableBase GetLangTable()
        {
            var localCfg = (UMLocalCfg)target;
            var umConfig = localCfg.GetComponentInParent<UMOConfig>();
            if (umConfig == null)
                return null;

            var tableDic = Field_UMConfig_TableDic?.GetValue(umConfig) as Dictionary<Type, UMConfigTableBase>;
            if (tableDic == null)
                return null;

            foreach (var table in tableDic.Values)
            {
                if (table is IUMLangTable)
                    return table;
            }
            return null;
        }

        private string GetLangAssetDir(UMConfigTableBase table)
        {
            if (table == null)
                return null;

            string assetPath = table.AssetPath;
            int lastSlash = assetPath.LastIndexOf('/');
            return lastSlash >= 0 ? assetPath.Substring(0, lastSlash) : assetPath;
        }
    }
}
