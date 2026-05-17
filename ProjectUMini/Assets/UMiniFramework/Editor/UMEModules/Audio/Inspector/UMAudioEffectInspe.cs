using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.Audio.Inspector
{
    [CustomEditor(typeof(UMAudioEffect))]
    public class UMAudioEffectInspe : UnityEditor.Editor
    {
        private bool m_foEffectClips = true; // 控制折叠状态
        private Dictionary<string, UMAudioClipInfo> m_effectClipDic;
        private static FieldInfo Field_UMAudioEffect_EffectClipDic;

        private void OnEnable()
        {
            Field_UMAudioEffect_EffectClipDic =
                UMEUtilCommon.GetObjectNoPublicField(typeof(UMAudioEffect), "m_effectClipDic");
            m_effectClipDic = (Dictionary<string, UMAudioClipInfo>) Field_UMAudioEffect_EffectClipDic.GetValue(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // EditorGUILayout.LabelField("Register Event Tags", EditorStyles.boldLabel);
            m_foEffectClips = EditorGUILayout.Foldout(m_foEffectClips, $"Effect Clips ({m_effectClipDic.Keys.Count})");
            if (m_foEffectClips)
            {
                // 绘制 Effect Clips
                EditorGUI.indentLevel++; // 增加缩进
                int effectIndex = 0;
                foreach (var kv in m_effectClipDic)
                {
                    string id = kv.Key;
                    EditorGUILayout.BeginVertical("helpbox");

                    // 绘制effect索引
                    string indexFormat = string.Format("{0:D4}", effectIndex);
                    EditorGUILayout.LabelField($"Index[{indexFormat}]", EditorStyles.boldLabel);

                    // 绘制 Effect ID
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Audio Effect ID: ", EditorStyles.boldLabel, GUILayout.Width(110));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(kv.Key);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制 Effect Info
                    UMAudioClipInfo clipInfo = kv.Value;

                    // if (clipInfo.ID != kv.Key)
                    // {
                    //     // 绘制Effect ID
                    //     EditorGUILayout.BeginHorizontal();
                    //     EditorGUILayout.LabelField($"Clip ID: ", EditorStyles.boldLabel,
                    //         GUILayout.Width(90));
                    //     // 禁用编辑
                    //     EditorGUI.BeginDisabledGroup(true);
                    //     EditorGUILayout.TextField(clipInfo.ID);
                    //     // 结束禁用组
                    //     EditorGUI.EndDisabledGroup();
                    //     EditorGUILayout.EndHorizontal();
                    // }

                    // 绘制effect路径
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Clip Path: ", EditorStyles.boldLabel,
                        GUILayout.Width(80));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(clipInfo.Path);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制effect路径类型
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Clip Path Type: ", EditorStyles.boldLabel,
                        GUILayout.Width(110));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(clipInfo.PathType.ToString());
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制effect 是否需要预加载
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Clip Is Preload: ", EditorStyles.boldLabel,
                        GUILayout.Width(110));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(clipInfo.IsPreLoad.ToString());
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制Clip对象
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Clip Object: ", EditorStyles.boldLabel,
                        GUILayout.Width(90));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(clipInfo.Clip, typeof(AudioClip));
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                    GUILayout.Space(5);
                    effectIndex++;
                }

                EditorGUI.indentLevel--; // 恢复缩进
            }
        }
    }
}