using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;
using UMiniFramework.Runtime.Modules.Event.Base;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules.Event
{
    public class UMEvent : UMBaseModule
    {
        private Dictionary<string, List<UMEventListener>> m_eventDic;
        private UMEventInitArgs m_initArgs = null;

        public override UMModuleType ModuleType
        {
            get => UMModuleType.Event;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            m_eventDic = new Dictionary<string, List<UMEventListener>>();
            m_initArgs = UMUtilCommon.ConvertObjectClass<UMEventInitArgs>(initArgs);

            if (m_initArgs != null
                && m_initArgs.RegisterEventTags != null)
            {
                for (var i = 0; i < m_initArgs.RegisterEventTags.Count; i++)
                {
                    string eventTag = m_initArgs.RegisterEventTags[i];
                    m_eventDic.Add(eventTag, new List<UMEventListener>());
                }
            }

            yield return null;
        }

        public void AddListener(UMEventListener listener)
        {
            if (!m_eventDic.Keys.Contains(listener.EventTag))
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
            if (!m_eventDic.Keys.Contains(eventTag))
            {
                UMUtilDebug.Warning($"Failed to dispatch. Event flag [{eventTag}] is not registered");
                return;
            }

            List<UMEventListener> eventTagListeners = m_eventDic[eventTag];

            for (var i = 0; i < eventTagListeners.Count; i++)
            {
                eventTagListeners[i].HandleEvent(content);
            }

            eventTagListeners.RemoveAll((listener) =>
            {
                // 移除只侦听一次的 listener
                return listener.ListenType == UMListenType.Once;
            });
        }

        public void RemoveListener(UMEventListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener), "The parameter cannot be null");
            }

            if (m_eventDic.Keys.Contains(listener.EventTag))
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
            if (m_eventDic.Keys.Contains(eventTag))
            {
                m_eventDic[eventTag].Clear();
            }
        }
    }
}