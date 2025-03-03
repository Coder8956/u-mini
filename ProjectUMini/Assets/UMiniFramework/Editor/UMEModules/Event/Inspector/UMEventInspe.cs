using System;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Editor.EUtils;
using UMiniFramework.Runtime.Modules.Event;
using UMiniFramework.Runtime.Modules.Event.EventContent.Base;
using UMiniFramework.Runtime.Modules.Event.Listener;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace UMiniFramework.Editor.UMEModules.Event.Inspector
{
    [CustomEditor(typeof(UMEvent))]
    public class UMEventInspe : UnityEditor.Editor
    {
        private bool m_foEventTags = true; // 控制折叠状态
        private bool[] m_foETListeners = null; // 控制折叠状态
        private Dictionary<string, List<UMEventListener>> m_eventDic;
        private static FieldInfo Field_UMEvent_EventDic;
        private static FieldInfo Field_UMEL_EventHandler;
        private GUIStyle m_listenerInfoGS;

        private void OnEnable()
        {
            Field_UMEvent_EventDic = UMEUtilCommon.GetObjectNoPublicField(typeof(UMEvent), "m_eventDic");
            Field_UMEL_EventHandler = UMEUtilCommon.GetObjectNoPublicField(typeof(UMEventListener), "EventHandler");

            m_eventDic = (Dictionary<string, List<UMEventListener>>) Field_UMEvent_EventDic.GetValue((UMEvent) target);
            m_foETListeners = new bool[m_eventDic.Keys.Count];

            m_listenerInfoGS = new GUIStyle("helpbox");
            m_listenerInfoGS.fontSize = 12;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // EditorGUILayout.LabelField("Register Event Tags", EditorStyles.boldLabel);
            m_foEventTags = EditorGUILayout.Foldout(m_foEventTags, $"Register Event Tags ({m_eventDic.Keys.Count})");
            if (m_foEventTags)
            {
                // 绘制 事件Tag
                EditorGUI.indentLevel++; // 增加缩进
                int tagIndex = 0;
                foreach (var kv in m_eventDic)
                {
                    string etag = kv.Key;
                    List<UMEventListener> listeners = kv.Value;
                    bool[] foListeners = new bool[listeners.Count]; // 控制折叠状态

                    EditorGUILayout.BeginHorizontal();

                    string tagIndexFormat = string.Format("{0:D4}", tagIndex);
                    EditorGUILayout.LabelField($"Index[{tagIndexFormat}] Event Tag:", EditorStyles.boldLabel,
                        GUILayout.Width(155));

                    // 禁用编辑
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextArea(etag);
                    // 结束禁用组
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.EndHorizontal();

                    // 绘制 事件Tag 下所有的侦听器
                    EditorGUI.indentLevel++; // 增加缩进
                    m_foETListeners[tagIndex] =
                        EditorGUILayout.Foldout(m_foETListeners[tagIndex], $"Listeners ({kv.Value.Count})");

                    if (m_foETListeners[tagIndex])
                    {
                        // 禁用编辑
                        EditorGUI.BeginDisabledGroup(true);

                        for (var i = 0; i < kv.Value.Count; i++)
                        {
                            UMEventListener listener = kv.Value[i];

                            // 获取事件处理器 EventHandler 是哪个类中方法
                            string ehInfo = string.Empty;
                            UnityAction<UMBaseEventContent> eh =
                                (UnityAction<UMBaseEventContent>) Field_UMEL_EventHandler.GetValue(listener);

                            if (eh == null)
                            {
                                ehInfo = "null";
                            }
                            else
                            {
                                Type declaringType = eh.Method.DeclaringType;
                                ehInfo = string.Concat(declaringType.FullName, ".", eh.Method.Name);
                            }

                            EditorGUILayout.TextArea($"Index: {i}\n" + // 绘制 listener 索引
                                                     $"HashCode: {listener.GetHashCode()}\n" + // 绘制 listener HashCode
                                                     $"Event Tag: {listener.EventTag}\n" + // 绘制 Event Tag
                                                     $"Listen Type: {listener.ListenType.ToString()}\n" + // 绘制 Listen Type
                                                     $"Event Handler: {ehInfo}", m_listenerInfoGS); // ; 绘制事件处理器信息
                        }

                        // 结束禁用组
                        EditorGUI.EndDisabledGroup();
                    }

                    EditorGUI.indentLevel--; // 恢复缩进
                    tagIndex++;
                }

                EditorGUI.indentLevel--; // 恢复缩进
            }
        }
    }
}