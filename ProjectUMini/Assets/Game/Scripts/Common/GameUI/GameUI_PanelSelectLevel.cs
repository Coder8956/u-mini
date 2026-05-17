using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        private static PanelSelectLevel PanelSelectLevel = null;

        public static void OpenSelectLevel()
        {
            if (PanelSelectLevel == null)
            {
                PanelSelectLevel = UMF.UI.Create<PanelSelectLevel>();
            }

            UMF.UI.Open(PanelSelectLevel);
        }

        public static void CloseSelectLevel()
        {
            if (PanelSelectLevel == null) return;
            UMF.UI.Close(PanelSelectLevel);
        }
    }
}