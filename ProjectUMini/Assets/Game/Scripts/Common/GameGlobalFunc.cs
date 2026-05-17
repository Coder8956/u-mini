using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.Scene;

namespace Game.Scripts.Common
{
    public class GameGlobalFunc
    {
        public static void BackMain()
        {
            GameUI.CloseGame();
            UMF.Get<UMScene>().Load(GameScene.Main);
        }
    }
}