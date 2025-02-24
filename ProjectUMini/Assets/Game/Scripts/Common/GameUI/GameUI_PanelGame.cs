using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common.GameUI
{
    public partial class GameUI
    {
        private static PanelGame PanelGame = null;

        /// <summary>
        /// 打开 设置界面
        /// </summary>
        public static void OpenGame()
        {
            if (PanelGame == null)
            {
                PanelGame = UMGR.Get<UMUI>().Create<PanelGame>();
            }

            UMGR.Get<UMUI>().Open(PanelGame);
        }

        /// <summary>
        /// 关闭 设置界面
        /// </summary>
        public static void CloseGame()
        {
            if (PanelGame == null) return;
            UMGR.Get<UMUI>().Close(PanelGame);
        }
    }
}