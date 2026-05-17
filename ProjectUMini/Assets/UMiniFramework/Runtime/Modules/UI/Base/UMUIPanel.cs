using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules
{
    public abstract class UMUIPanel : MonoBehaviour
    {
        private bool m_isOpen = false;
        [SerializeField] private bool m_isUseCommonMask = true;

        public bool IsUseCommonMask
        {
            get => m_isUseCommonMask;
            set => m_isUseCommonMask = value;
        }

        public Image PanelMask
        {
            get { return GetComponent<Image>(); }
        }

        /// <summary>
        /// UI是否是打开的(打开=true.关闭=false)
        /// </summary>
        public bool IsOpen => m_isOpen;

        /// <summary>
        /// 在创建的时候执行一次
        /// </summary>
        protected abstract void OnCreatePanel();

        /// <summary>
        /// 在销毁的时候执行一次
        /// </summary>
        protected abstract void OnDestroyPanel();

        /// <summary>
        /// 每次打开的时候执行
        /// </summary>
        protected abstract void OnOpenPanel();

        /// <summary>
        /// 每次关闭的时候执行
        /// </summary>
        protected abstract void OnClosePanel();
    }
}