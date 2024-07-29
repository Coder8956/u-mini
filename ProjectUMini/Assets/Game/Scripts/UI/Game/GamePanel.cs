using Game.Scripts.Const;
using UMiniFramework.Runtime.UMEntrance;
using UMiniFramework.Runtime.Modules.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Game
{
    [UMUIPanelInfo("UI/Game/GamePanel")]
    public class GamePanel : UMUIPanel
    {
        [SerializeField] private Button m_btnBackLogin;

        public override void OnLoaded()
        {
            m_btnBackLogin.onClick.AddListener(BackLogin);
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }

        private void BackLogin()
        {
            UMini.Audio.BGM.Stop();
            UMini.Scene.Load(SceneConst.LOGIN);
            UMini.UI.Close<GamePanel>();
        }
    }
}