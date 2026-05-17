using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
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
                PanelGameResult = UMF.UI.Create<PanelGameResult>();
            }

            UMF.UI.Open(PanelGameResult);
            PanelGameResult.OnAgain = onAgain;
            PanelGameResult.OnBackMain = onBackMain;
        }

        public static void CloseGameResult()
        {
            if (PanelGameResult == null) return;
            UMF.UI.Close(PanelGameResult);
            PanelGameResult.OnAgain = null;
        }
    }
}