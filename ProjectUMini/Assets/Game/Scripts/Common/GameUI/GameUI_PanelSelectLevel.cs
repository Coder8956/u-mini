using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        private static PanelSelectLevel PanelSelectLevel = null;

        public static void OpenSelectLevel()
        {
            if (PanelSelectLevel == null)
            {
                PanelSelectLevel = UMGR.Get<UMUI>().Create<PanelSelectLevel>();
            }

            UMGR.Get<UMUI>().Open(PanelSelectLevel);
        }
        
        public static void CloseSelectLevel()
        {
            if (PanelSelectLevel == null) return;
            UMGR.Get<UMUI>().Close(PanelSelectLevel);
        }
    }
}