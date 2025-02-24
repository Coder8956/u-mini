using Game.Scripts.Common;
using Game.Scripts.Common.GameUI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Scene;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelConfig("UI/PanelGame/PanelGame")]
    public class PanelGame : UMUIPanel
    {
        [SerializeField] private Button m_btnBackMain;

        protected override void OnCreatePanel()
        {
            m_btnBackMain.onClick.AddListener(() =>
            {
                GameUI.CloseGame();
                UMGR.Get<UMScene>().Load(GameScene.Main);
            });
        }

        protected override void OnDestroyPanel()
        {
        }

        protected override void OnOpenPanel()
        {
        }

        protected override void OnClosePanel()
        {
        }
    }
}