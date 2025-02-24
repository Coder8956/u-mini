using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common.GameUI
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
                PanelSet = UMGR.Get<UMUI>().Create<PanelSet>();
            }

            UMGR.Get<UMUI>().Open(PanelSet);
        }

        /// <summary>
        /// 关闭 设置界面
        /// </summary>
        public static void CloseSet()
        {
            if (PanelSet == null) return;
            UMGR.Get<UMUI>().Close(PanelSet);
        }
    }
}