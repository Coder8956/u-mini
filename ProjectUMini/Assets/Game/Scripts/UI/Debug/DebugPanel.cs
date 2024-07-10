using System;
using Game.Scripts.UI.Login;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Modules.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Debug
{
    [UMUIPanelInfo("UI/Debug/DebugPanel")]
    public class DebugPanel : UMUIPanel
    {
        [SerializeField] private Button m_btnOpenLogin;

        public override void OnLoaded()
        {
            m_btnOpenLogin.onClick.AddListener(() => { UMini.UI.Open<LoginPanel>(); });
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }
    }
}