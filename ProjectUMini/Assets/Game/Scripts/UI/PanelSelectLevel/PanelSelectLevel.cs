using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Scene;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Modules.UI.UMUIAttribute;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelATB("UI/PanelSelectLevel/PanelSelectLevel")]
    public class PanelSelectLevel : UMUIPanel
    {
        [SerializeField] private Button m_btnClose;
        [SerializeField] private Button m_btnLevel_1;
        [SerializeField] private Button m_btnLevel_2;
        [SerializeField] private Button m_btnLevel_3;

        private void EnterGame(string levelId)
        {
            GameGlobalVar.SelectLevelId = levelId;
            UMGR.Get<UMScene>().Load(GameScene.Game);
            GameUI.CloseMain();
            GameUI.CloseSelectLevel();
        }

        protected override void OnCreatePanel()
        {
            m_btnClose.onClick.AddListener(() => { GameUI.CloseSelectLevel(); });
            m_btnLevel_1.onClick.AddListener(() => { EnterGame("level_11001"); });
            m_btnLevel_2.onClick.AddListener(() => { EnterGame("level_11002"); });
            m_btnLevel_3.onClick.AddListener(() => { EnterGame("level_11003"); });
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