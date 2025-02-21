using Game.Scripts.UI.PanelMain;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;

namespace Game.Scripts.Common
{
    public class GameUI
    {
        private static PanelMain M_PanelMain = null;

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
    }
}