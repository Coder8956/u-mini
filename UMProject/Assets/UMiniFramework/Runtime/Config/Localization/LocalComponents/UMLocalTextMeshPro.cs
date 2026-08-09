using System;
using TMPro;
using UnityEngine;

namespace UMiniFramework.Runtime
{
    [RequireComponent(typeof(TMP_Text))]
    public class UMLocalTextMeshPro : UMLocalComponent
    {
        private TMP_Text m_text;
        private Func<string, string> m_localTextHandler;

        public string text => m_text != null ? m_text.text : string.Empty;

        protected override void OnAwake()
        {
            m_text = GetComponent<TMP_Text>();
            if (m_text == null)
            {
                Debug.LogWarning($"UMLocalTextMeshPro m_text is null. localID:{m_localID}");
            }
        }

        internal override void OnUpdateLocal()
        {
            if (m_text == null) return;

            m_text.text = m_localTextHandler != null
                ? m_localTextHandler.Invoke(LocalValue())
                : LocalValue();
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
