using Game.Scripts.UI.PanelDebug;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common.GameUI
{
    public partial class GameUI
    {
        private static PanelDebug PanelDebug = null;

        /// <summary>
        /// 打开 Debug 界面
        /// </summary>
        public static void OpenDebug()
        {
            if (PanelDebug == null)
            {
                PanelDebug = UMGR.Get<UMUI>().Create<PanelDebug>();
            }

            int topLayer = UMGR.Get<UMUI>().TopLayerIndex;
            UMGR.Get<UMUI>().Open(PanelDebug, topLayer);
        }

        /// <summary>
        /// 关闭 Debug 界面
        /// </summary>
        public static void CloseDebug()
        {
            if (PanelDebug == null) return;
            UMGR.Get<UMUI>().Close(PanelDebug);
        }
    }
}