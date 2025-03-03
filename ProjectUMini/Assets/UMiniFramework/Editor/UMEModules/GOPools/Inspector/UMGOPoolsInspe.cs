using System;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules.Event;
using UMiniFramework.Runtime.Modules.GOPools;
using UMiniFramework.Runtime.Modules.GOPools.Pool;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.GOPools.Inspector
{
    [CustomEditor(typeof(UMGOPools))]
    public class UMGOPoolsInspe : UnityEditor.Editor
    {
        private Dictionary<string, UMGOP> m_poolDic;
        private static FieldInfo Field_UMGOPools_PoolDic;
        private bool m_foPools = true; // 控制折叠状态
        private bool[] m_foPoolInfo = null; // 控制折叠状态

        private void OnEnable()
        {
            Field_UMGOPools_PoolDic = UMEUtilCommon.GetObjectNoPublicField(typeof(UMGOPools), "m_poolDic");
            m_poolDic = (Dictionary<string, UMGOP>) Field_UMGOPools_PoolDic.GetValue((UMGOPools) target);

            m_foPoolInfo = new bool[m_poolDic.Keys.Count];
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            m_foPools = EditorGUILayout.Foldout(m_foPools, "Pools");
            if (m_foPools)
            {
                // 绘制 事件Tag
                EditorGUI.indentLevel++; // 增加缩进
                int poolIndex = 0;
                foreach (var kv in m_poolDic)
                {
                    string ptag = kv.Key;
                    UMGOP pool = kv.Value;

                    EditorGUILayout.BeginHorizontal();

                    string tagIndexFormat = string.Format("{0:D4}", poolIndex);
                    EditorGUILayout.LabelField($"Pool Tag {tagIndexFormat}", EditorStyles.boldLabel,
                        GUILayout.Width(110));

                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(kv.Value.gameObject, typeof(UMGOP));
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.EndHorizontal();

                    poolIndex++;
                }

                EditorGUI.indentLevel--; // 恢复缩进
            }
        }
    }
}