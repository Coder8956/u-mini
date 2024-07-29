using UMiniFramework.Runtime.UMEntrance;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules.UIModule
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        [SerializeField] private bool m_setBtnClosePanel = false;

        protected bool SetBtnClosePanel
        {
            get => m_setBtnClosePanel;
            set => m_setBtnClosePanel = value;
        }

        [SerializeField] private Button m_btnClosePanel = null;

        protected Button BtnClose
        {
            get => m_btnClosePanel;
            set => m_btnClosePanel = value;
        }

        protected virtual void Awake()
        {
            if (m_setBtnClosePanel && m_btnClosePanel != null)
            {
                m_btnClosePanel.onClick.AddListener(OnClickBtnClose);
            }
        }

        protected virtual void OnClickBtnClose()
        {
            UMini.UI.Close(this);
        }

        public abstract void OnLoaded();
        public abstract void OnOpen();
        public abstract void OnClose();
    }
}