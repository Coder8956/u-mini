using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        private static PanelSet PanelSet = null;

        /// <summary>
        /// 打开 设置界面
        /// </summary>
        public static void OpenSet()
        {
            if (PanelSet == null)
            {
                PanelSet = UMF.UI.Create<PanelSet>();
            }

            UMF.UI.Open(PanelSet);
        }

        /// <summary>
        /// 关闭 设置界面
        /// </summary>
        public static void CloseSet()
        {
            if (PanelSet == null) return;
            UMF.UI.Close(PanelSet);
        }
    }
}