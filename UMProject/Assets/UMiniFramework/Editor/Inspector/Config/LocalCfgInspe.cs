using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor
{
    [CustomEditor(typeof(LocalCfg))]
    public class LocalCfgInspe : UnityEditor.Editor
    {
        private bool m_foLocal = true; // 多语言对象折叠状态
        private bool m_foOptions = true; // 语言选项折叠状态
        private Vector2 m_scrollPos;

        private static readonly FieldInfo Field_UMConfig_TableDic =
            typeof(UMConfig).GetField("m_tableDic", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo Field_LocalCfg_LocalComponents =
            typeof(LocalCfg).GetField("m_localComponents", BindingFlags.NonPublic | BindingFlags.Instance);

        private const int ScrollThreshold = 10;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocalCfg localCfg = (LocalCfg)target;

            m_foLocal = EditorGUILayout.Foldout(m_foLocal, "Localization (LocalCfg)");
            if (!m_foLocal)
                return;

            EditorGUI.indentLevel++;

            // Current Type
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Type:", EditorStyles.boldLabel, GUILayout.Width(130));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(localCfg.CurtType ?? "(none)");
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // Current Code
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Code:", EditorStyles.boldLabel, GUILayout.Width(130));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(localCfg.CurtCode ?? "(none)");
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

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

                UMBaseConfigTable baseTable = GetLangTable();
                IUMLangTable langTable = baseTable as IUMLangTable;
                string langDir = GetLangAssetDir(baseTable);

                for (int i = 0; i < optionCount; i++)
                {
                    var opt = options[i];
                    string fileName = langTable?.GetLanguageFile(i);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(opt.type);
                    EditorGUILayout.LabelField(opt.code ?? "?", GUILayout.Width(60));
                    EditorGUILayout.LabelField(fileName ?? "?", GUILayout.Width(100));
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Ping", GUILayout.Width(50)))
                    {
                        PingLangAsset(langDir, fileName);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (optionCount > ScrollThreshold)
                {
                    EditorGUILayout.EndScrollView();
                }

                EditorGUI.indentLevel--;
            }

            // Registered Components
            int componentCount = 0;
            if (Field_LocalCfg_LocalComponents != null)
            {
                var list = Field_LocalCfg_LocalComponents.GetValue(localCfg) as IList;
                if (list != null)
                    componentCount = list.Count;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Registered Components:", EditorStyles.boldLabel, GUILayout.Width(130));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField(componentCount);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

        private void PingLangAsset(string langDir, string fileName)
        {
            if (string.IsNullOrEmpty(langDir) || string.IsNullOrEmpty(fileName))
            {
                Debug.LogWarning("[LocalCfgInspe] Failed to resolve language asset path.");
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
                Debug.LogWarning($"[LocalCfgInspe] Language asset not found: {filePath}");
            }
        }

        private UMBaseConfigTable GetLangTable()
        {
            var localCfg = (LocalCfg)target;
            var umConfig = localCfg.GetComponentInParent<UMConfig>();
            if (umConfig == null)
                return null;

            var tableDic = Field_UMConfig_TableDic?.GetValue(umConfig) as Dictionary<Type, UMBaseConfigTable>;
            if (tableDic == null)
                return null;

            foreach (var table in tableDic.Values)
            {
                if (table is IUMLangTable)
                    return table;
            }
            return null;
        }

        private string GetLangAssetDir(UMBaseConfigTable table)
        {
            if (table == null)
                return null;

            string assetPath = table.AssetPath;
            int lastSlash = assetPath.LastIndexOf('/');
            return lastSlash >= 0 ? assetPath.Substring(0, lastSlash) : assetPath;
        }
    }
}
