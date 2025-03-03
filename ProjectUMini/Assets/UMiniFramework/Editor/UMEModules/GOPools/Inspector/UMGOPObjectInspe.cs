using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules.GOPools.Pool;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.GOPools.Inspector
{
    [CustomEditor(typeof(UMGOPObject))]
    public class UMGOPObjectInspe : UnityEditor.Editor
    {
        private static FieldInfo Field_UMGOPObject_BornPool;
        private UMGOP m_bornPoolObject;

        private void OnEnable()
        {
            Field_UMGOPObject_BornPool = UMEUtilCommon.GetObjectNoPublicField(typeof(UMGOPObject), "m_bornPool");
            m_bornPoolObject = (UMGOP) Field_UMGOPObject_BornPool.GetValue(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Born Pool Object: ", EditorStyles.boldLabel,
                GUILayout.Width(120));
            // 禁用编辑
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(m_bornPoolObject, typeof(UMGOP));
            // 结束禁用组
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }
    }
}