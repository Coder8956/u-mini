using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime
{
    public abstract class UMUIPanelBase : MonoBehaviour
    {
        [SerializeField] private bool m_useCommonMask = true;
        [SerializeField] private RectTransform[] m_layoutRebuilderOnOpen;
        [SerializeField] private Button m_btnClose;

        public bool UseCommonMask
        {
            get { return m_useCommonMask; }
            // set { m_useCommonMask = value; }
        }

        protected Image m_imgMask;

        private int m_layer;

        internal void Initialize()
        {
            m_imgMask = GetComponent<Image>();
            if (m_btnClose != null)
            {
                m_btnClose.onClick.AddListener(Close);
            }

            OnInitialize();
        }

        /// <summary>
        /// 打开面板：挂到对应层级 → 拉伸 → 激活 → 回调。
        /// </summary>
        public virtual void Open(int layer = 0)
        {
            m_layer = layer;
            transform.SetParent(UMUI.GetLayer(m_layer), false);
            UMUIUtils.StretchFull(GetComponent<RectTransform>());

            if (UseCommonMask && m_imgMask != null)
            {
                m_imgMask.color = UMUI.PanelMaskColor;
            }

            gameObject.SetActive(true);

            if (m_layoutRebuilderOnOpen != null)
            {
                for (var i = 0; i < m_layoutRebuilderOnOpen.Length; i++)
                {
                    RectTransform rebuildRect = m_layoutRebuilderOnOpen[i];
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rebuildRect);
                }
            }
        }

        /// <summary>
        /// 关闭面板：回调 → 失活 → 移入缓存。
        /// </summary>
        public virtual void Close()
        {
            if (this == null) return;
            gameObject.SetActive(false);
            transform.SetParent(UMUI.UICache, false);
        }

        /// <summary>
        /// 释放面板：回调 → 移除缓存 → 销毁。
        /// </summary>
        public virtual void Release()
        {
            if (gameObject.activeSelf)
            {
                Close();
            }

            UMUI.RemoveFromCache(this);
            Destroy(gameObject);
        }

        protected abstract void OnInitialize();
    }
}