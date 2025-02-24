using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelConfig("UI/PanelSet/PanelSet")]
    public class PanelSet : UMUIPanel
    {
        [SerializeField] private Button m_btnClose;

        protected override void OnCreatePanel()
        {
            m_btnClose.onClick.AddListener(() => { GameUI.CloseSet(); });
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
            GameUI.SetMaskColor(gameObject);
        }

        protected override void OnClosePanel()
        {
        }
    }
}