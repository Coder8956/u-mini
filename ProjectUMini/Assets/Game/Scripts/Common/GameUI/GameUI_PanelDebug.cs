using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;

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
                PanelDebug = UMF.UI.Create<PanelDebug>();
            }

            int topLayer = UMF.UI.TopLayerIndex;
            UMF.UI.Open(PanelDebug, topLayer);
        }

        /// <summary>
        /// 关闭 Debug 界面
        /// </summary>
        public static void CloseDebug()
        {
            if (PanelDebug == null) return;
            UMF.UI.Close(PanelDebug);
        }
    }
}