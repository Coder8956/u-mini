using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Scripts.UMEntrance;
using UMiniFramework.Scripts.Utils;

namespace UMiniFramework.Scripts.Modules.EventModule
{
    public class UMEventModule : UMModule
    {
        private Dictionary<string, List<UMListenerInfo>> MessageDic;

        public override IEnumerator Init(UMiniConfig config)
        {
            MessageDic = new Dictionary<string, List<UMListenerInfo>>();
            if (config.MessageEventList != null)
            {
                for (var i = 0; i < config.MessageEventList.Count; i++)
                {
                    string eventType = config.MessageEventList[i];
                    MessageDic.Add(eventType, new List<UMListenerInfo>());
                }
            }

            yield return null;
        }

        public void AddListener(string eventType, IUMEventListener listener,
            UMListenType type = UMListenType.Persistent)
        {
            if (MessageDic.Keys.Contains(eventType))
            {
                if (listener == null)
                {
                    UMUtils.Debug.Warning($"AddListener listener is null.");
                }
                else
                {
                    bool isRepeatListener = MessageDic[eventType].Exists((lObject) => lObject.Listener == listener);
                    if (isRepeatListener)
                    {
                        UMUtils.Debug.Warning(
                            $"Stop add listener, because added a duplicate listener. HashCode: {listener.GetHashCode()}");
                    }
                    else
                    {
                        MessageDic[eventType].Add(new UMListenerInfo(type, listener));
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
            if (MessageDic.Keys.Contains(umEvent.Type))
            {
                for (var i = 0; i < MessageDic[umEvent.Type].Count; i++)
                {
                    UMListenerInfo listenerInfo = MessageDic[umEvent.Type][i];
                    listenerInfo.Listener.UMOnReceiveMessage(umEvent);
                }
            }
        }
    }
}