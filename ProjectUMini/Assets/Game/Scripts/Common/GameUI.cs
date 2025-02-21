using Game.Scripts.UI.PanelDebug;
using Game.Scripts.UI.PanelMain;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common
{
    public class GameUI
    {
        private static PanelMain M_PanelMain = null;

        /// <summary>
        /// 主界面
        /// </summary>
        public static PanelMain PanelMain
        {
            get
            {
                if (M_PanelMain == null)
                {
                    M_PanelMain = UMGR.Get<UMUI>().Create<PanelMain>();
                }

                return M_PanelMain;
            }
        }

        private static PanelDebug M_PanelDebug = null;

        public static void OpenDebug()
        {
            // 打开 debug 界面

            if (M_PanelDebug == null)
            {
                M_PanelDebug = UMGR.Get<UMUI>().Create<PanelDebug>();
            }

            int topLayer = UMGR.Get<UMUI>().TopLayerIndex;
            UMGR.Get<UMUI>().Open(M_PanelDebug, topLayer);
        }
    }
}