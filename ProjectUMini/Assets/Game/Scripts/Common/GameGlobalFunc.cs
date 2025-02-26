using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Scene;

namespace Game.Scripts.Common
{
    public class GameGlobalFunc
    {
        public static void BackMain()
        {
            GameUI.CloseGame();
            UMGR.Get<UMScene>().Load(GameScene.Main);
        }
    }
}