using UMiniFramework.Runtime.Modules.Manager;

namespace Game.Scripts.Common
{
    public class GameGlobalFunc
    {
        public static void BackMain()
        {
            GameUI.CloseGame();
            UMF.Scene.Load(GameScene.Main);
        }
    }
}