using Game.Scripts.UI;
using UMiniFramework.Runtime.Modules.Manager;
using UMiniFramework.Runtime.Modules.UI;
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
                PanelGame = UMF.Get<UMUI>().Create<PanelGame>();
            }

            UMF.Get<UMUI>().Open(PanelGame);
            PanelGame.OnBackMain = onBackMain;
        }

        public static void CloseGame()
        {
            if (PanelGame == null) return;
            UMF.Get<UMUI>().Close(PanelGame);
        }
    }
}