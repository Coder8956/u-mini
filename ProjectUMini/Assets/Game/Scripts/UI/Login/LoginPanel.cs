using Game.Scripts.Const;
using Game.Scripts.Gameplay;
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
            m_btnLevel_1.onClick.AddListener(OnClickLevel_1);
            m_btnLevel_2.onClick.AddListener(OnClickLevel_2);
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }

        private void OnClickLevel_1()
        {
            EnterGame("level_11001");
        }

        private void OnClickLevel_2()
        {
            EnterGame("level_11002");
        }

        private void EnterGame(string levelId)
        {
            GameMain.SetGameLevel(levelId);
            UMini.Scene.Load(SceneConst.GAME);
            UMini.UI.Close<LoginPanel>();
        }
    }
}