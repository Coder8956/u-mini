using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        private static PanelMain PanelMain = null;

        /// <summary>
        /// 打开 主界面
        /// </summary>
        public static void OpenMain()
        {
            if (PanelMain == null)
            {
                PanelMain = UMGR.Get<UMUI>().Create<PanelMain>();
            }

            UMGR.Get<UMUI>().Open(PanelMain);
        }

        /// <summary>
        /// 关闭 主界面
        /// </summary>
        public static void CloseMain()
        {
            if (PanelMain == null) return;
            UMGR.Get<UMUI>().Close(PanelMain);
        }
    }
}