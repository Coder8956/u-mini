using Game.Scripts.Common;
using Game.Scripts.GameEvent;
using UMiniFramework.Runtime.Modules.Event;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI.Base;
using UMiniFramework.Runtime.Modules.UI.UMUIAttribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelATB("UI/PanelGameResult/PanelGameResult")]
    public class PanelGameResult : UMUIPanel
    {
        [SerializeField] private Button m_btnAgain;
        [SerializeField] private Button m_btnBackMain;

        public UnityAction OnAgain { get; set; }
        public UnityAction OnBackMain { get; set; }

        protected override void OnCreatePanel()
        {
            m_btnAgain.onClick.AddListener(() =>
            {
                UMGR.Get<UMEvent>().Dispatch(GameEventTags.GameAgain);
                OnAgain?.Invoke();
                GameUI.CloseGameResult();
            });

            m_btnBackMain.onClick.AddListener(() =>
            {
                OnBackMain?.Invoke();
                GameGlobalFunc.BackMain();
                GameUI.CloseGameResult();
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