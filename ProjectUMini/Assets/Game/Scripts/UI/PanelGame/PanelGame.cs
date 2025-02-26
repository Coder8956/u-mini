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
        [SerializeField] private Text m_txtLevelId;

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
            m_txtLevelId.text = string.Format("Level Id: {0}", GameGlobalVar.SelectLevelId);
        }

        protected override void OnClosePanel()
        {
        }
    }
}