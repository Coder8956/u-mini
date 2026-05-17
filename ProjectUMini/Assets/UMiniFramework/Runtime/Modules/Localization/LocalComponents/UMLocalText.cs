using System;
using UMiniFramework.Runtime.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules
{
    [RequireComponent(typeof(Text))]
    public class UMLocalText : UMLocalComponent
    {
        private Text m_text;
        private Func<string, string> m_localTextHandler;

        public string text
        {
            get { return m_text.text; }
        }

        protected override void OnAwake()
        {
            m_text = GetComponent<Text>();
            if (m_text == null)
            {
                UMUtilDebug.Warning($"UMLocalText m_text is null. localID:{m_localID}");
            }
        }

        protected override void OnUpdateLocal()
        {
            if (m_text == null) return;
            if (m_localTextHandler != null)
            {
                m_text.text = m_localTextHandler.Invoke(LocalValue());
            }
            else
            {
                m_text.text = LocalValue();
            }
        }

        public void SetLocalTextHandler(Func<string, string> handler, bool immeUpdate = true)
        {
            m_localTextHandler = handler;
            if (immeUpdate)
            {
                OnUpdateLocal();
            }
        }

        public void ClearLocalTextHandle()
        {
            m_localTextHandler = null;
        }
    }
}