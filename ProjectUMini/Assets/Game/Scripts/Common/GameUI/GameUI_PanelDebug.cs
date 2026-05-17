using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common
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
                PanelDebug = UMF.Get<UMUI>().Create<PanelDebug>();
            }

            int topLayer = UMF.Get<UMUI>().TopLayerIndex;
            UMF.Get<UMUI>().Open(PanelDebug, topLayer);
        }

        /// <summary>
        /// 关闭 Debug 界面
        /// </summary>
        public static void CloseDebug()
        {
            if (PanelDebug == null) return;
            UMF.Get<UMUI>().Close(PanelDebug);
        }
    }
}