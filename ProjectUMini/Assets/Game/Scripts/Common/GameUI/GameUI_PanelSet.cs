using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

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
                PanelSet = UMF.Get<UMUI>().Create<PanelSet>();
            }

            UMF.Get<UMUI>().Open(PanelSet);
        }

        /// <summary>
        /// 关闭 设置界面
        /// </summary>
        public static void CloseSet()
        {
            if (PanelSet == null) return;
            UMF.Get<UMUI>().Close(PanelSet);
        }
    }
}