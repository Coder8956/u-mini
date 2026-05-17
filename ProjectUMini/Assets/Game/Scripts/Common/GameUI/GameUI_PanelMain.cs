using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;

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
                PanelMain = UMF.UI.Create<PanelMain>();
            }

            UMF.UI.Open(PanelMain);
        }

        /// <summary>
        /// 关闭 主界面
        /// </summary>
        public static void CloseMain()
        {
            if (PanelMain == null) return;
            UMF.UI.Close(PanelMain);
        }
    }
}