using UMiniFramework.Scripts.Pool.GameObjectPool;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMInspectorEditor.GameObjectPoolInspector
{
    [CustomEditor(typeof(UMGameObjectPool))]
    [CanEditMultipleObjects]
    public class UMGameObjectPoolEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            UMGameObjectPool GoPool = (UMGameObjectPool) target;

            // 绘制默认的 Inspector GUI（包括其他字段）
            DrawDefaultInspector();

            GUI.enabled = false;
            // 使用反射访问和编辑属性
            EditorGUILayout.LabelField("ObjectCount", GoPool.ObjectCount.ToString());
            GUI.enabled = true;

            // 保存更改
            if (GUI.changed)
            {
                EditorUtility.SetDirty(GoPool);
            }
        }
    }
}