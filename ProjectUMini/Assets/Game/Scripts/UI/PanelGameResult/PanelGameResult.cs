using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelConfig("UI/PanelGameResult/PanelGameResult")]
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