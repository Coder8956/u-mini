using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Utils;

namespace UMiniFramework.Runtime.Modules.EventModule
{
    public class UMEventModule : UMModule
    {
        private Dictionary<string, List<UMListenerInfo>> EventDic;

        public override IEnumerator Init(UMiniConfig config)
        {
            EventDic = new Dictionary<string, List<UMListenerInfo>>();
            if (config.EventTypeList != null)
            {
                for (var i = 0; i < config.EventTypeList.Count; i++)
                {
                    string eventType = config.EventTypeList[i];
                    EventDic.Add(eventType, new List<UMListenerInfo>());
                }
            }

            yield return null;
            m_initFinished = true;
            UMUtilCommon.PrintModuleInitFinishedLog(GetType().Name, m_initFinished);
        }

        public void AddListener(string eventType, IUMEventListener listener,
            UMListenType type = UMListenType.Persistent)
        {
            if (EventDic.Keys.Contains(eventType))
            {
                if (listener == null)
                {
                    UMUtilDebug.Warning($"AddListener listener is null.");
                }
                else
                {
                    bool isRepeatListener = EventDic[eventType].Exists((lObject) => lObject.Listener == listener);
                    if (isRepeatListener)
                    {
                        UMUtilDebug.Warning(
                            $"Stop add listener, because added a duplicate listener. HashCode: {listener.GetHashCode()}");
                    }
                    else
                    {
                        EventDic[eventType].Add(new UMListenerInfo(type, listener));
                    }
                }
            }
            else
            {
                UMUtilDebug.Warning($"Invalid message type:{eventType}.");
            }
        }

        public void Dispatch(string eventType, UMEventBody eventBody = null)
        {
            UMEvent umEvent = new UMEvent(eventType, eventBody);
            if (EventDic.Keys.Contains(umEvent.Type))
            {
                for (var i = 0; i < EventDic[umEvent.Type].Count; i++)
                {
                    UMListenerInfo listenerInfo = EventDic[umEvent.Type][i];
                    listenerInfo.Listener.UMOnReceiveEvent(umEvent);
                }

                EventDic[umEvent.Type].RemoveAll((info) => info.Type == UMListenType.Once);
            }
        }

        public void RemoveListener(string eventType, IUMEventListener listener)
        {
            if (EventDic.Keys.Contains(eventType))
            {
                EventDic[eventType].RemoveAll((info) => info.Listener == listener);
            }
        }

        public void RemoveAllListener()
        {
            foreach (var infoList in EventDic.Values)
            {
                infoList.Clear();
            }
        }

        public void RemoveAllListenerByEvnetType(string eventType)
        {
            if (EventDic.Keys.Contains(eventType))
            {
                EventDic[eventType].Clear();
            }
        }
    }
}