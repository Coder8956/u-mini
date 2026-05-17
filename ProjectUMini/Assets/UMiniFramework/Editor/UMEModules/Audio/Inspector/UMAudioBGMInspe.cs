using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules;
using UnityEditor;
using UnityEngine;

namespace UMiniFramework.Editor.UMEModules.Audio.Inspector
{
    [CustomEditor(typeof(UMAudioBGM))]
    public class UMAudioBGMInspe : UnityEditor.Editor
    {
        private bool m_foBGMClips = true; // 控制折叠状态
        private Dictionary<string, UMAudioClipInfo> m_BGMClipDic;
        private static FieldInfo Field_UMAudioBGM_BGMClipDic;

        private void OnEnable()
        {
            Field_UMAudioBGM_BGMClipDic = UMEUtilCommon.GetObjectNoPublicField(typeof(UMAudioBGM), "m_BGMClipDic");
            m_BGMClipDic = (Dictionary<string, UMAudioClipInfo>) Field_UMAudioBGM_BGMClipDic.GetValue(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // EditorGUILayout.LabelField("Register Event Tags", EditorStyles.boldLabel);
            m_foBGMClips = EditorGUILayout.Foldout(m_foBGMClips, $"BGM Clips ({m_BGMClipDic.Keys.Count})");
            if (m_foBGMClips)
            {
                // 绘制 BGM Clips
                EditorGUI.indentLevel++; // 增加缩进
                int bgmIndex = 0;
                foreach (var kv in m_BGMClipDic)
                {
                    string id = kv.Key;
                    EditorGUILayout.BeginVertical("helpbox");

                    // 绘制bgm索引
                    string indexFormat = string.Format("{0:D4}", bgmIndex);
                    EditorGUILayout.LabelField($"Index[{indexFormat}]", EditorStyles.boldLabel);

                    // 绘制 BGM ID
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Audio BGM ID: ", EditorStyles.boldLabel, GUILayout.Width(110));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(kv.Key);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制 BGM Info
                    UMAudioClipInfo clipInfo = kv.Value;

                    // if (clipInfo.ID != kv.Key)
                    // {
                    //     // 绘制bgm ID
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

                    // 绘制bgm路径
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Clip Path: ", EditorStyles.boldLabel,
                        GUILayout.Width(80));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(clipInfo.Path);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制bgm路径类型
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Clip Path Type: ", EditorStyles.boldLabel,
                        GUILayout.Width(110));
                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(clipInfo.PathType.ToString());
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();

                    // 绘制bgm 是否需要预加载
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
                    bgmIndex++;
                }

                EditorGUI.indentLevel--; // 恢复缩进
            }
        }
    }
}