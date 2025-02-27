using Game.Scripts.Common;
using UMiniFramework.Runtime.Modules.UI;
using UMiniFramework.Runtime.Modules.UI.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [UMUIPanelConfig("UI/PanelGame/PanelGame")]
    public class PanelGame : UMUIPanel
    {
        [SerializeField] private Button m_btnBackMain;
        [SerializeField] private Text m_txtLevelId;

        public UnityAction OnBackMain { get; set; }

        protected override void OnCreatePanel()
        {
            m_btnBackMain.onClick.AddListener(() =>
            {
                OnBackMain?.Invoke();
                GameGlobalFunc.BackMain();
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