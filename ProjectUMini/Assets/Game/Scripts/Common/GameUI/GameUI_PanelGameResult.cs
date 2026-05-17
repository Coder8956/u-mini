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
                PanelGameResult = UMF.Get<UMUI>().Create<PanelGameResult>();
            }

            UMF.Get<UMUI>().Open(PanelGameResult);
            PanelGameResult.OnAgain = onAgain;
            PanelGameResult.OnBackMain = onBackMain;
        }

        public static void CloseGameResult()
        {
            if (PanelGameResult == null) return;
            UMF.Get<UMUI>().Close(PanelGameResult);
            PanelGameResult.OnAgain = null;
        }
    }
}