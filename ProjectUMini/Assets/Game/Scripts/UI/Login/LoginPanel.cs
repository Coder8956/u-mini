using Game.Scripts.Const;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Modules.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Login
{
    [UMUIPanelInfo("UI/Login/LoginPanel")]
    public class LoginPanel : UMUIPanel
    {
        [SerializeField] private Button m_btnLevel_1;
        [SerializeField] private Button m_btnLevel_2;

        public override void OnLoaded()
        {
            m_btnLevel_1.onClick.AddListener(EnterGame);
            m_btnLevel_2.onClick.AddListener(EnterGame);
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }

        private void EnterGame()
        {
            UMini.Scene.Load(SceneConst.GAME);
            UMini.UI.Close<LoginPanel>();
        }
    }
}