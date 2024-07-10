using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Modules.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Login
{
    [UMUIPanelInfo("UI/Login/LoginPanel")]
    public class LoginPanel : UMUIPanel
    {
        [SerializeField] private Button m_btnClose;

        public override void OnLoaded()
        {
            m_btnClose.onClick.AddListener(() => { UMini.UI.Close<LoginPanel>(); });
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }
    }
}