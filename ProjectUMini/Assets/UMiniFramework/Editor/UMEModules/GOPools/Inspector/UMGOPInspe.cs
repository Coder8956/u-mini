using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.GOPools.Inspector
{
    [CustomEditor(typeof(UMGOP))]
    public class UMGOPInspe : UnityEditor.Editor
    {
        private static FieldInfo Field_UMGOP_Tag;
        private static FieldInfo Field_UMGOP_GOQue;
        private static FieldInfo Field_UMGOP_OutPoolGOs;

        private void OnEnable()
        {
            Field_UMGOP_Tag = UMEUtilCommon.GetObjectNoPublicField(typeof(UMGOP), "m_poolTag");
            Field_UMGOP_GOQue = UMEUtilCommon.GetObjectNoPublicField(typeof(UMGOP), "m_goQue");
            Field_UMGOP_OutPoolGOs = UMEUtilCommon.GetObjectNoPublicField(typeof(UMGOP), "m_outPoolGos");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            string poolTag = (string) Field_UMGOP_Tag.GetValue(target);
            EditorGUILayout.LabelField($"Pool Tag: {poolTag}", EditorStyles.boldLabel);

            Queue<GameObject> inPoolObjects = (Queue<GameObject>) Field_UMGOP_GOQue.GetValue(target);
            EditorGUILayout.LabelField($"In Pool Object: {inPoolObjects.Count}", EditorStyles.boldLabel);

            List<GameObject> outPoolObjects = (List<GameObject>) Field_UMGOP_OutPoolGOs.GetValue(target);
            EditorGUILayout.LabelField($"Out Pool Object: {outPoolObjects.Count}", EditorStyles.boldLabel);
        }
    }
}