using System;
using System.Collections.Generic;

namespace UMiniFramework.Runtime
{
    public class UMEvent : UMMonoSingleton<UMEvent>
    {
        private Dictionary<string, List<UMEventListener>> m_eventDic;

        protected override void OnInit()
        {
            m_eventDic = new Dictionary<string, List<UMEventListener>>();
        }

        public static void AddEvent(string eventTag)
        {
            if (Instance.m_eventDic.ContainsKey(eventTag)) return;
            Instance.m_eventDic.Add(eventTag, new List<UMEventListener>());
        }

        public static void AddListener(UMEventListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener), "The parameter cannot be null");
            }

            if (!Instance.m_eventDic.TryGetValue(listener.EventTag, out var listeners))
            {
                return;
            }

            if (listeners.Contains(listener))
            {
                return;
            }

            listeners.Add(listener);
        }

        public static void Dispatch(string eventTag, UMBaseEventContent content = null)
        {
            if (!Instance.m_eventDic.TryGetValue(eventTag, out var listeners))
            {
                return;
            }

            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                UMEventListener listener = listeners[i];
                listener.HandleEvent(content);
                if (listener.ListenType == UMListenType.Once)
                {
                    listeners.RemoveAt(i);
                }
            }
        }

        public static void RemoveListener(UMEventListener listener)
        {
            if (listener == null) return;
            if (Instance.m_eventDic.TryGetValue(listener.EventTag, out var listeners))
            {
                listeners.Remove(listener);
            }
        }

        public static void RemoveAllListener()
        {
            foreach (var listeners in Instance.m_eventDic.Values)
            {
                listeners.Clear();
            }
        }

        public static void RemoveAllListenerByEventTag(string eventTag)
        {
            if (Instance.m_eventDic.TryGetValue(eventTag, out var listeners))
            {
                listeners.Clear();
            }
        }
    }
}