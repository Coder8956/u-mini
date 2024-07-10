using Game.Scripts.UI.Login;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Modules.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Debug
{
    [UMUIPanelInfo("UI/Debug/DebugPanel", UMUILayer.TOP)]
    public class DebugPanel : UMUIPanel
    {
        [SerializeField] private Button m_btnShowDebugFunc;
        [SerializeField] private Button m_btnOpenLogin;
        [SerializeField] private RectTransform m_debugFuncRoot;

        public override void OnLoaded()
        {
            m_btnShowDebugFunc.onClick.AddListener(() =>
            {
                m_debugFuncRoot.gameObject.SetActive(!m_debugFuncRoot.gameObject.activeSelf);
            });

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