using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UnityEngine.Events;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        private static PanelGame PanelGame = null;

        public static void OpenGame(UnityAction onBackMain = null)
        {
            if (PanelGame == null)
            {
                PanelGame = UMF.UI.Create<PanelGame>();
            }

            UMF.UI.Open(PanelGame);
            PanelGame.OnBackMain = onBackMain;
        }

        public static void CloseGame()
        {
            if (PanelGame == null) return;
            UMF.UI.Close(PanelGame);
        }
    }
}