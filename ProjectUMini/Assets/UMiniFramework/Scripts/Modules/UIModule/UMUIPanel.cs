using UMiniFramework.Scripts.UMEntrance;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Scripts.Modules.UIModule
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        [SerializeField] private bool m_isHasBtnClose = false;

        protected bool IsHasBtnClose
        {
            get => m_isHasBtnClose;
            set => m_isHasBtnClose = value;
        }

        [SerializeField] private Button m_btnClose = null;

        protected Button BtnClose
        {
            get => m_btnClose;
            set => m_btnClose = value;
        }

        protected virtual void Awake()
        {
            if (m_isHasBtnClose && m_btnClose != null)
            {
                m_btnClose.onClick.AddListener(OnClickBtnClose);
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