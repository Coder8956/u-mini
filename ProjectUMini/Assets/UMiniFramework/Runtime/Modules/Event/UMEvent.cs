using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules
{
    public class UMEvent : UMBaseModule
    {
        private Dictionary<string, List<UMEventListener>> m_eventDic;
        private MethodInfo m_listenerHandleEventMethod = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Event;
        }

        protected override IEnumerator Init()
        {
            m_listenerHandleEventMethod = UMUtilCommon.GetObjectNoPublicMethod(typeof(UMEventListener), "HandleEvent");
            m_eventDic = new Dictionary<string, List<UMEventListener>>();
            UMUtilDebug.Log($"{GetType().Name} Inited");

            yield return null;
        }

        public void AddEvent(string eventTag)
        {
            if (m_eventDic.ContainsKey(eventTag)) return;
            m_eventDic.Add(eventTag, new List<UMEventListener>());
        }

        public void AddListener(UMEventListener listener)
        {
            if (!m_eventDic.ContainsKey(listener.EventTag))
            {
                UMUtilDebug.Warning($"Failed to add listener. Event flag [{listener.EventTag}] is not registered");
                return;
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener), "The parameter cannot be null");
            }

            bool isDuplicateListener = m_eventDic[listener.EventTag].Exists((lObject) => lObject == listener);

            if (isDuplicateListener)
            {
                UMUtilDebug.Warning($"Failed to add a listener, because added a duplicate listener. ");
                return;
            }

            m_eventDic[listener.EventTag].Add(listener);
        }

        public void Dispatch(string eventTag, UMBaseEventContent content = null)
        {
            if (!m_eventDic.ContainsKey(eventTag))
            {
                UMUtilDebug.Warning($"Failed to dispatch. Event flag [{eventTag}] is not registered");
                return;
            }

            List<UMEventListener> eventTagListeners = m_eventDic[eventTag];

            for (var i = eventTagListeners.Count - 1; i >= 0; i--)
            {
                UMEventListener listener = eventTagListeners[i];
                m_listenerHandleEventMethod.Invoke(listener, new object[] {content});
                if (listener.ListenType == UMListenType.Once)
                {
                    eventTagListeners.Remove(listener);
                }
            }
        }

        public void RemoveListener(UMEventListener listener)
        {
            if (listener == null) return;
            if (m_eventDic.ContainsKey(listener.EventTag))
            {
                m_eventDic[listener.EventTag].Remove(listener);
            }
        }

        public void RemoveAllListener()
        {
            foreach (var listeners in m_eventDic.Values)
            {
                listeners.Clear();
            }
        }

        public void RemoveAllListenerByEvnetTag(string eventTag)
        {
            if (m_eventDic.ContainsKey(eventTag))
            {
                m_eventDic[eventTag].Clear();
            }
        }
    }
}