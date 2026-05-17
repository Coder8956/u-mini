using System;
using UnityEngine.Events;

namespace UMiniFramework.Runtime.Modules
{
    public enum UMListenType
    {
        /// <summary>
        /// 侦听一次
        /// </summary>
        Once,

        /// <summary>
        /// 持续侦听
        /// </summary>
        Persistent
    }

    public sealed class UMEventListener
    {
        public readonly string EventTag;
        public readonly UMListenType ListenType;
        private readonly UnityAction<UMBaseEventContent> EventHandler;

        public UMEventListener(string eventTag, UnityAction<UMBaseEventContent> eventHandler,
            UMListenType listenType = UMListenType.Persistent)
        {
            EventTag = eventTag;
            ListenType = listenType;
            EventHandler = eventHandler;
            if (EventHandler == null)
            {
                throw new ArgumentNullException(nameof(EventHandler), "The parameter cannot be null");
            }
        }

        private void HandleEvent(UMBaseEventContent content)
        {
            if (EventHandler != null)
            {
                EventHandler.Invoke(content);
            }
            else
            {
                throw new ArgumentNullException(nameof(EventHandler), "Valid parameters are missing");
            }
        }
    }
}