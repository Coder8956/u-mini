using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.PanelMain
{
    [UMUIPanelConfig("UI/PanelMain/PanelMain")]
    public class PanelMain : UMUIPanel
    {
        [SerializeField] private Button m_btnClose;
        [SerializeField] private Button m_btnDestroy;

        protected override void OnCreatePanel()
        {
            // Debug.Log("Create-PanelMain");
            m_btnClose.onClick.AddListener(() =>
            {
                // 关闭界面
                UMGR.Get<UMUI>().Close(this);
            });

            m_btnDestroy.onClick.AddListener(() =>
            {
                // 销毁界面
                UMGR.Get<UMUI>().Destroy(this);
            });
        }

        protected override void OnDestroyPanel()
        {
            Debug.Log("Destroy-PanelMain");
        }

        protected override void OnOpenPanel()
        {
            Debug.Log("Open-PanelMain");
        }

        protected override void OnClosePanel()
        {
            Debug.Log("Close-PanelMain");
        }
    }
}