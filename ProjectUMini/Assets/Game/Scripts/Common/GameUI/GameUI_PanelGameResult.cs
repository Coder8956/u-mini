using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
using UnityEngine.Events;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        private static PanelGameResult PanelGameResult = null;

        public static void OpenGameResult(UnityAction onAgain = null, UnityAction onBackMain = null)
        {
            if (PanelGameResult == null)
            {
                PanelGameResult = UMGR.Get<UMUI>().Create<PanelGameResult>();
            }

            UMGR.Get<UMUI>().Open(PanelGameResult);
            PanelGameResult.OnAgain = onAgain;
        }

        public static void CloseGameResult()
        {
            if (PanelGameResult == null) return;
            UMGR.Get<UMUI>().Close(PanelGameResult);
            PanelGameResult.OnAgain = null;
        }
    }
}