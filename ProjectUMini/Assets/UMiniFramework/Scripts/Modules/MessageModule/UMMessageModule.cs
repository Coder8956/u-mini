using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMiniFramework.Scripts.Utils;

namespace UMiniFramework.Scripts.Modules.MessageModule
{
    public class UMMessageModule : UMModule
    {
        private Dictionary<string, List<UMListenObject>> MessageDic;

        public override IEnumerator Init(UMini.UMiniConfig config)
        {
            MessageDic = new Dictionary<string, List<UMListenObject>>();
            if (config.MessageEventList != null)
            {
                for (var i = 0; i < config.MessageEventList.Count; i++)
                {
                    string eventType = config.MessageEventList[i];
                    MessageDic.Add(eventType, new List<UMListenObject>());
                }
            }

            yield return null;
        }

        public void AddListener(string eventType, IMessageListener listener, ListenType type = ListenType.Persistent)
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
                        UMUtils.Debug.Warning($"Repeat Listener: {listener.GetHashCode()}");
                    }
                    else
                    {
                        MessageDic[eventType].Add(new UMListenObject(type, listener));
                    }
                }
            }
            else
            {
                UMUtils.Debug.Warning($"Invalid message type:{eventType}.");
            }
        }
    }
}