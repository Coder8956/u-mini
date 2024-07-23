using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Utils;

namespace UMiniFramework.Scripts.Modules.EventModule
{
    public class UMEventModule : UMModule
    {
        private Dictionary<string, List<UMListenerInfo>> EventDic;

        public override IEnumerator Init(UMiniConfig config)
        {
            EventDic = new Dictionary<string, List<UMListenerInfo>>();
            if (config.MessageEventList != null)
            {
                for (var i = 0; i < config.MessageEventList.Count; i++)
                {
                    string eventType = config.MessageEventList[i];
                    EventDic.Add(eventType, new List<UMListenerInfo>());
                }
            }

            yield return null;
        }

        public void AddListener(string eventType, IUMEventListener listener,
            UMListenType type = UMListenType.Persistent)
        {
            if (EventDic.Keys.Contains(eventType))
            {
                if (listener == null)
                {
                    UMUtils.Debug.Warning($"AddListener listener is null.");
                }
                else
                {
                    bool isRepeatListener = EventDic[eventType].Exists((lObject) => lObject.Listener == listener);
                    if (isRepeatListener)
                    {
                        UMUtils.Debug.Warning(
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
                UMUtils.Debug.Warning($"Invalid message type:{eventType}.");
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