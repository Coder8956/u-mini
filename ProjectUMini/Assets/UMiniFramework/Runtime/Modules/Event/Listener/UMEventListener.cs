using System;
using UMiniFramework.Runtime.Modules.Event.EventContent.Base;
using UnityEngine.Events;

namespace UMiniFramework.Runtime.Modules.Event.Listener
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

        public UMEventListener(string eventTag, UMListenType listenType, UnityAction<UMBaseEventContent> eventHandler)
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