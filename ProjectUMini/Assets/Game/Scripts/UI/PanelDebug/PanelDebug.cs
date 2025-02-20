using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.PanelDebug
{
    [UMUIPanelConfig("UI/PanelMain/PanelDebug")]
    public class PanelDebug : UMUIPanel
    {
        [SerializeField] private Button m_btnOpenPanelMain = null;

        protected override void OnCreatePanel()
        {
            // Debug.Log("Create-PanelDebug");
            m_btnOpenPanelMain?.onClick.AddListener(() =>
            {
                // 打开主界面
                UMGR.Get<UMUI>().Open(GameUI.PanelMain, 5);
            });
            // UMGR.Get<UMUI>().Open(pGame);
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            Debug.Log("Open-PanelDebug");
        }

        protected override void OnClosePanel()
        {
        }
    }
}